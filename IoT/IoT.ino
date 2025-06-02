#include <WiFiNINA.h>
#include <ArduinoHttpClient.h>
#include <Arduino_JSON.h>
#include <Arduino_HTS221.h>
#include <Arduino_APDS9960.h>
#include <Wire.h>
#include <Arduino_MKRIoTCarrier.h>
MKRIoTCarrier carrier;

WiFiServer server(80);

char ssid[32] = "";
char pass[32] = "";
bool wifiReady = false;

char serverAddress[] = "10.133.51.109";
int port = 80;

const int soilMoisturePin = A5;
char plantGUID[18];

WiFiClient wifi;
HttpClient client = HttpClient(wifi, serverAddress, port);

APDS9960 lightSensor(Wire, -1);

// ----------- URL decode function -------------
String urlDecode(const String &str)
{
  String decoded = "";
  char temp[] = "0x00";
  unsigned int len = str.length();
  unsigned int i = 0;
  while (i < len)
  {
    char c = str[i];
    if (c == '+')
    {
      decoded += ' ';
    }
    else if (c == '%' && i + 2 < len)
    {
      temp[2] = str[i + 1];
      temp[3] = str[i + 2];
      decoded += (char)strtol(temp, NULL, 16);
      i += 2;
    }
    else
    {
      decoded += c;
    }
    i++;
  }
  return decoded;
}

void setup()
{
  Serial.begin(9600);
  while (!Serial)
    ;

  CARRIER_CASE = false;
  carrier.begin();
  showSplashScreen();

  if (!lightSensor.begin())
  {
    Serial.println("APDS9960 initialization failed!");
    while (1)
      ;
  }
  Serial.println("APDS9960 ready!");

  wifiReady = false;
  loadWiFiSettings();

  // --- WiFi connect block ---
  if (strlen(ssid) > 0 && strlen(pass) > 0)
  {
    Serial.print("Trying to connect to WiFi: ");
    Serial.println(ssid);
    WiFi.begin(ssid, pass);
    int tries = 0;
    while (WiFi.status() != WL_CONNECTED && tries < 30)
    {
      delay(1000);
      Serial.print(".");
      tries++;
    }
    Serial.println();
    if (WiFi.status() == WL_CONNECTED)
    {
      wifiReady = true;
      Serial.print("Connected to WiFi. IP Address: ");
      Serial.println(WiFi.localIP());
    }
  }

  // --- If not connected, go into AP mode ---
  while (!wifiReady)
  {
    // Show available WiFi networks for easier debugging/input
    scanAndPrintWiFiNetworks();

    WiFi.beginAP("PotPal_Setup");
    IPAddress ip = WiFi.localIP();
    Serial.print("AP IP address: ");
    Serial.println(ip);

    server.begin();
    carrier.display.fillScreen(ST77XX_BLACK);
    carrier.display.setTextColor(ST77XX_WHITE);
    carrier.display.setTextSize(2);
    carrier.display.setCursor(10, carrier.display.height() / 2 - 10);
    carrier.display.print("Config WiFi via AP");

    bool gotConfig = false;
    while (!wifiReady)
    {
      WiFiClient client = server.available();
      if (client)
      {
        gotConfig = handleWiFiConfigClient(client);
        if (gotConfig)
        {
          Serial.print("Trying to connect to WiFi: ");
          Serial.println(ssid);
          WiFi.end();
          delay(1000);
          WiFi.begin(ssid, pass);
          int tries = 0;
          while (tries < 30)
          {
            int status = WiFi.status();
            Serial.print(status);
            if (status == WL_CONNECTED)
            {
              Serial.print("Successfull connected");
              break;
            }
            delay(1000);
            Serial.print(".");
            tries++;
          }
          Serial.println();
          if (WiFi.status() == WL_CONNECTED)
          {
            wifiReady = true;
            Serial.print("Connected to WiFi. IP Address: ");
            Serial.println(WiFi.localIP());
          }
          else
          {
            Serial.println("Failed to connect. Staying in AP mode.");
          }
        }
      }
    }
  }

  // ---- Sensors initialization ----
  if (!HTS.begin())
  {
    carrier.display.print("Failed to initialize humidity and temperature sensor!");
    Serial.println("Failed to initialize humidity and temperature sensor!");
    while (1)
      ;
  }

  byte mac[6];
  WiFi.macAddress(mac);
  sprintf(plantGUID, "%02X:%02X:%02X:%02X:%02X:%02X",
          mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
  Serial.print("GUID: ");
  Serial.println(plantGUID);
}

void loop()
{
  if (wifiReady)
  {
    mainPotPalLogic();
  }
}

// ------- WiFi Configurator Handler -------
bool handleWiFiConfigClient(WiFiClient &client)
{
  String req = "";
  while (client.connected())
  {
    if (client.available())
    {
      char c = client.read();
      req += c;
      if (c == '\n' && req.endsWith("\r\n\r\n"))
        break;
    }
  }

  // Show web form for WiFi credentials
  if (req.indexOf("GET") >= 0)
  {
    client.println("HTTP/1.1 200 OK");
    client.println("Content-Type: text/html");
    client.println("Connection: close");
    client.println();
    client.println("<!DOCTYPE html><html><body>");
    client.println("<h2>PotPal WiFi Setup</h2>");
    client.println("<form method='POST'>SSID:<br><input name='ssid'><br>Password:<br><input name='pass' type='password'><br><input type='submit' value='Connect'></form>");
    client.println("</body></html>");
  }

  // Handle form submission (POST)
  if (req.indexOf("POST") >= 0)
  {
    while (client.available() == 0)
      delay(1);
    String postData = "";
    while (client.available())
      postData += (char)client.read();

    int ssidIndex = postData.indexOf("ssid=");
    int passIndex = postData.indexOf("&pass=");
    if (ssidIndex >= 0 && passIndex > ssidIndex)
    {
      String ssidVal = postData.substring(ssidIndex + 5, passIndex);
      String passVal = postData.substring(passIndex + 6);

      ssidVal = urlDecode(ssidVal);
      passVal = urlDecode(passVal);
      ssidVal.trim();
      passVal.trim();

      Serial.print("Submitted SSID: [");
      Serial.print(ssidVal);
      Serial.println("]");
      Serial.print("Submitted PASS: [");
      Serial.print(passVal);
      Serial.println("]");

      ssidVal.toCharArray(ssid, 32);
      passVal.toCharArray(pass, 32);

      Serial.print("Submitted SSID: [");
      Serial.print(ssid);
      Serial.println("]");
      Serial.print("Submitted PASS: [");
      Serial.print(pass);
      Serial.println("]");

      client.println("HTTP/1.1 200 OK");
      client.println("Content-Type: text/html");
      client.println("Connection: close");
      client.println();
      client.println("<html><body><h2>Trying to connect... Please wait for LED/Serial.</h2>");
      client.println("<p>If not connecting, reconnect to 'PotPal_Setup' and try again.</p></body></html>");
      delay(1000);
      client.stop();
      Serial.println("Config received from user.");
      return true;
    }
  }
  delay(10);
  client.stop();
  Serial.println("Client disconnected");
  return false;
}

// ------- WiFi settings loader (stub, can be expanded) -------
void loadWiFiSettings()
{
  ssid[0] = 0;
  pass[0] = 0;
}

// ------- WiFi scanner (prints available networks in Serial) -------
void scanAndPrintWiFiNetworks()
{
  Serial.println("Scanning for WiFi networks...");
  int n = WiFi.scanNetworks();
  if (n == 0)
  {
    Serial.println("No networks found.");
  }
  else
  {
    Serial.print(n);
    Serial.println(" networks found:");
    for (int i = 0; i < n; ++i)
    {
      Serial.print("  ");
      Serial.print(WiFi.SSID(i));
      Serial.print(" (");
      Serial.print(WiFi.RSSI(i));
      Serial.print(" dBm)");
      Serial.println((WiFi.encryptionType(i) == ENC_TYPE_NONE) ? " (open)" : " (secured)");
      delay(10);
    }
  }
  Serial.println("------");
}

// ------- Main PotPal logic: sensor reading, display, backend -------
void mainPotPalLogic()
{
  // Reconnect to WiFi if connection is lost
  if (WiFi.status() != WL_CONNECTED)
  {
    carrier.display.print("WiFi disconnected. Reconnecting...");
    Serial.println("WiFi disconnected. Reconnecting...");
    WiFi.begin(ssid, pass);
    int tries = 0;
    while (WiFi.status() != WL_CONNECTED && tries < 30)
    {
      delay(1000);
      Serial.print(".");
      tries++;
    }
    Serial.println();
    if (WiFi.status() != WL_CONNECTED)
    {
      Serial.println("Failed to reconnect to WiFi.");
      return;
    }
  }

  // Read soil moisture as integer percentage
  int soilRaw = analogRead(soilMoisturePin);
  int soilPercent = convertToPercentage(soilRaw);

  // Read air temperature and humidity from HTS221
  float airTemperature = HTS.readTemperature();
  float airHumidity = HTS.readHumidity();

  // Read ambient light from APDS9960 (c = clear channel = ambient light)
  int r = 0, g = 0, b = 0, c = 0;
  if (lightSensor.colorAvailable())
  {
    lightSensor.readColor(r, g, b, c);
    Serial.print("Ambient light: ");
    Serial.println(c);
  }
  else
  {
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
  data["SoilMoisture"] = soilPercent;
  data["Temperature"] = airTemperature;
  data["AirHumidity"] = int(airHumidity);
  data["LightLevel"] = c;

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

// ----------- Helper functions -----------
int convertToPercentage(int rawValue)
{
  int percent = map(rawValue, 790, 365, 0, 100);
  percent = constrain(percent, 0, 100);
  return percent;
}

void printCentered(const char *text, int y, int textSize)
{
  int16_t x;
  int textLength = strlen(text);
  int charWidth = 6;
  x = (carrier.display.width() - textLength * charWidth * textSize) / 2;
  carrier.display.setCursor(x, y);
  carrier.display.setTextSize(textSize);
  carrier.display.print(text);
}

void updateDisplay(int soil, int airHum, int light, int airTemp)
{
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

void showSplashScreen()
{
  carrier.display.fillScreen(ST77XX_BLACK);
  carrier.display.setTextColor(ST77XX_WHITE);
  carrier.display.setTextSize(3);
  int16_t x = (carrier.display.width() - 8 * 7 * 3) / 2;
  int16_t y = (carrier.display.height() - 16 * 3) / 2;
  carrier.display.setCursor(x, y);
  carrier.display.print("PotPal");
  delay(2000);
}

void showWiFiStatus(const char *message)
{
  carrier.display.fillScreen(ST77XX_BLACK);
  carrier.display.setTextColor(ST77XX_WHITE);
  carrier.display.setTextSize(2);
  int16_t x = 10, y = carrier.display.height() / 2 - 10;
  carrier.display.setCursor(x, y);
  carrier.display.print(message);
}
