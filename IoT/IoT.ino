#include <WiFiNINA.h>
#include <ArduinoHttpClient.h>
#include <Arduino_JSON.h>
#include <Arduino_HTS221.h> // Library for built-in temperature and humidity sensor

// WiFi settings
char ssid[] = "Datahouse WIFI OLC";
char pass[] = "Merc1234!";

// API server settings
char serverAddress[] = "10.133.51.109"; // your server's local IP address
int port = 5000; // server port where your ASP.NET Core API is running

// Sensors
const int soilMoisturePin = A1; // Pin connected to soil moisture sensor

WiFiClient wifi;
HttpClient client = HttpClient(wifi, serverAddress, port);

void setup() {
  Serial.begin(9600);

  connectToWiFi();

  // Initialize HTS221 sensor
  if (!HTS.begin()) {
    Serial.println("Failed to initialize humidity and temperature sensor!");
    while (1); // Stop execution if sensor is not found
  }
}

void loop() {
  // Check WiFi connection and reconnect if needed
  if (WiFi.status() != WL_CONNECTED) {
    Serial.println("WiFi disconnected. Reconnecting...");
    connectToWiFi();
  }

  // Read sensor values
  int soilMoisture = analogRead(soilMoisturePin);
  float airTemperature = 0.0;
  float airHumidity = 0.0;

  readBuiltInSensors(airTemperature, airHumidity);

  // Print to Serial Monitor for debugging
  Serial.print("Soil moisture: ");
  Serial.println(soilMoisture);
  Serial.print("Air temperature: ");
  Serial.print(airTemperature);
  Serial.println(" °C");
  Serial.print("Air humidity: ");
  Serial.print(airHumidity);
  Serial.println(" %");

  // Create JSON payload
  JSONVar data;
  data["sensorType"] = "soil";
  data["soilMoisture"] = soilMoisture;
  data["airTemperature"] = airTemperature;
  data["airHumidity"] = airHumidity;

  String jsonString = JSON.stringify(data);

  // Send HTTP POST request
  client.beginRequest();
  client.post("/api/sensordata");
  client.sendHeader("Content-Type", "application/json");
  client.sendHeader("Content-Length", jsonString.length());
  client.beginBody();
  client.print(jsonString);
  client.endRequest();

  // Read response from server
  int statusCode = client.responseStatusCode();
  String response = client.responseBody();

  Serial.print("Status code: ");
  Serial.println(statusCode);
  Serial.print("Response: ");
  Serial.println(response);

  // Wait for 1 hour before next measurement
  delay(3600000); // 3600000 ms = 1 hour
}

void connectToWiFi() {
  Serial.print("Connecting to WiFi...");
  while (WiFi.begin(ssid, pass) != WL_CONNECTED) {
    Serial.print(".");
    delay(1000);
  }
  Serial.println();
  Serial.print("Connected to WiFi. IP Address: ");
  Serial.println(WiFi.localIP());
}

void readBuiltInSensors(float &temperature, float &humidity) {
  temperature = HTS.readTemperature(); // Read air temperature in °C
  humidity = HTS.readHumidity();       // Read air humidity in %
}
