#include <WiFiNINA.h>
#include <ArduinoHttpClient.h>
#include <Arduino_JSON.h>
#include <Arduino_HTS221.h>
#include <Arduino_APDS9960.h>
#include <Wire.h>
#include <Arduino_MKRIoTCarrier.h>
MKRIoTCarrier carrier;

// WiFi settings
char ssid[] = "Datahouse WIFI OLC";
char pass[] = "Merc1234!";

// API server settings
char serverAddress[] = "10.133.51.109"; // Backend IP
int port = 80;                        // Backend port

const int soilMoisturePin = A5;         // Analog pin for soil sensor
char plantGUID[18];                     // MAC address as plant GUID

WiFiClient wifi;
HttpClient client = HttpClient(wifi, serverAddress, port);

APDS9960 lightSensor(Wire, -1); // -1 means no INT pin used

void setup() {
  Serial.begin(9600);
  while (!Serial);

  CARRIER_CASE = false;
  carrier.begin();
  showSplashScreen();

  // Initialize APDS9960 light/color sensor
  if (!lightSensor.begin()) {
    Serial.println("APDS9960 initialization failed!");
    while (1);
  }
  Serial.println("APDS9960 ready!");

  // Connect to WiFi network
  connectToWiFi();

  // Initialize HTS221 temperature/humidity sensor
  if (!HTS.begin()) {
    carrier.display.print("Failed to initialize humidity and temperature sensor!");
    Serial.println("Failed to initialize humidity and temperature sensor!");
    while (1);
  }

  // Get MAC address as unique Plant GUID
  byte mac[6];
  WiFi.macAddress(mac);
  sprintf(plantGUID, "%02X:%02X:%02X:%02X:%02X:%02X",
          mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
  Serial.print("GUID: ");
  Serial.println(plantGUID);
}

void loop() {
  // Reconnect to WiFi if connection is lost
  if (WiFi.status() != WL_CONNECTED) {
    carrier.display.print("WiFi disconnected. Reconnecting...");
    Serial.println("WiFi disconnected. Reconnecting...");
    connectToWiFi();
  }

  // Read soil moisture as integer percentage
  int soilRaw = analogRead(soilMoisturePin);
  int soilPercent = convertToPercentage(soilRaw);

  // Read air temperature and humidity from HTS221
  float airTemperature = HTS.readTemperature();
  float airHumidity = HTS.readHumidity();

  // Read ambient light from APDS9960 (c = clear channel = ambient light)
  int r = 0, g = 0, b = 0, c = 0;
  if (lightSensor.colorAvailable()) {
    lightSensor.readColor(r, g, b, c);
    Serial.print("Ambient light: ");
    Serial.println(c);
  } else {
    Serial.println("No light data available");
  }

  // Print debug information to Serial Monitor
  Serial.print("Soil moisture: ");
  Serial.print(soilRaw);
  Serial.print(" -> ");
  Serial.print(soilPercent);
  Serial.println(" %");

  // Convert air humidity and temperature to integers for display
  int airHumidityInt = int(airHumidity);
  int airTempInt = int(airTemperature);

  // Update MKR IoT Carrier display with current values
  updateDisplay(soilPercent, airHumidityInt, c, airTempInt);

  // --- Build JSON payload for backend ---
  JSONVar data;
  data["PlantGUID"] = String(plantGUID);
  data["SoilMoisture"] = soilPercent;    // Integer
  data["Temperature"] = airTemperature;
  data["AirHumidity"] = int(airHumidity);
  data["LightLevel"] = c;                // Integer (clear channel)

  String jsonString = JSON.stringify(data);

  Serial.print("Sending: ");
  Serial.println(jsonString);

  // --- Send HTTP POST to backend ---
  client.beginRequest();
  client.post("/api/metric");
  client.sendHeader("Content-Type", "application/json");
  client.sendHeader("Content-Length", jsonString.length());
  client.beginBody();
  client.print(jsonString);
  client.endRequest();

  int statusCode = client.responseStatusCode();
  String response = client.responseBody();

  Serial.print("Status code: ");
  Serial.println(statusCode);
  Serial.print("Response: ");
  Serial.println(response);

  delay(60000); // Wait 1 minute between measurements
}

// Connect to WiFi network and show status on display
void connectToWiFi() {
  showWiFiStatus("Connecting to WiFi...");
  Serial.print("Connecting to WiFi...");
  while (WiFi.begin(ssid, pass) != WL_CONNECTED) {
    Serial.print(".");
    delay(1000);
  }
  Serial.println();
  Serial.print("Connected to WiFi. IP Address: ");
  Serial.println(WiFi.localIP());
}

// Convert raw soil sensor value to percent (dry = 790, wet = 365)
int convertToPercentage(int rawValue) {
  int percent = map(rawValue, 790, 365, 0, 100);
  percent = constrain(percent, 0, 100);
  return percent;
}

// Helper: print centered text on display at (y, with given text size)
void printCentered(const char* text, int y, int textSize) {
  int16_t x;
  int textLength = strlen(text);
  int charWidth = 6; // Standard Adafruit font width per character
  x = (carrier.display.width() - textLength * charWidth * textSize) / 2;
  carrier.display.setCursor(x, y);
  carrier.display.setTextSize(textSize);
  carrier.display.print(text);
}

// Update the MKR IoT Carrier display with latest sensor values
void updateDisplay(int soil, int airHum, int light, int airTemp) {
  carrier.display.fillScreen(ST77XX_BLACK);
  carrier.display.setTextColor(ST77XX_WHITE);

  char buf[32];

  sprintf(buf, "Soil: %d %%", soil);
  printCentered(buf, 20, 2);

  sprintf(buf, "Air Hum: %d %%", airHum);
  printCentered(buf, 50, 2);

  sprintf(buf, "Temp: %d C", airTemp);
  printCentered(buf, 80, 2);

  sprintf(buf, "Light: %d", light);
  printCentered(buf, 110, 2);
}

// Show splash screen at startup
void showSplashScreen() {
  carrier.display.fillScreen(ST77XX_BLACK);
  carrier.display.setTextColor(ST77XX_WHITE);
  carrier.display.setTextSize(3); // Large font
  // Calculate X/Y for center
  int16_t x = (carrier.display.width() - 8 * 7 * 3) / 2; // 8 px per char, 7 chars, size 3
  int16_t y = (carrier.display.height() - 16 * 3) / 2;   // 16 px font height, size 3
  carrier.display.setCursor(x, y);
  carrier.display.print("PotPal");
  delay(2000); // Show splash screen for 2 seconds
}

// Show current WiFi status message on display
void showWiFiStatus(const char* message) {
  carrier.display.fillScreen(ST77XX_BLACK);
  carrier.display.setTextColor(ST77XX_WHITE);
  carrier.display.setTextSize(2);
  int16_t x = 10, y = carrie_
