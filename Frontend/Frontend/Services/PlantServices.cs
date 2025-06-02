using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System;
using System.Text;
using Frontend.Components.Pages;

namespace Frontend.Services
{
    public class PlantServices
    {
        HttpClient Http = new HttpClient();

        protected string APIURL = "https://localhost:7192/api/Plant/";
        public async Task<Plant> PlantData(string plantGuid, string token)
        {
            try
            {
                // using to limit the scope of the HttpClient to this method.
                using (HttpClient client = new())
                {
                    // Add the authorization header to the request with the given token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Send a GET request to fetch user data from the "GetUsers" endpoint.
                    HttpResponseMessage response = await client.GetAsync($"https://localhost:7192/api/Plant/byGuid?guid={plantGuid}");

                    // Check if the request was successful.
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response content into a list of UserDTO objects.
                        Plant plantData = JsonConvert.DeserializeObject<Plant>(await response.Content.ReadAsStringAsync());

                        // Return the list of users if deserialization was successful.
                        if (plantData != null)
                        {
                            return plantData;
                        }
                    }
                }

            }
            catch { }
            return null;
        }

        public async Task<bool> UpdatePlantInformationAsync(Plant plantData, string token)
        {
            try
            {
                // using to limit the scope of the HttpClient to this method.
                using (HttpClient client = new())
                {
                    if (plantData == null || string.IsNullOrEmpty(token))
                    {
                        return false;
                    }

                    string plant = JsonConvert.SerializeObject(plantData);
                    StringContent data = new StringContent(plant, Encoding.UTF8, "application/json");
                    // Add the authorization header to the request with the given token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Send a GET request to fetch user data from the "GetUsers" endpoint.
                    HttpResponseMessage response = await client.PutAsync("https://localhost:7192/api/Plant/UpdatePlant", data);

                    // Check if the request was successful.
                    if (response.IsSuccessStatusCode)
                    {
                        return true;

                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
