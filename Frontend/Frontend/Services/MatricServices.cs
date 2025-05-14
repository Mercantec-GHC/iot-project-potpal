using Models;
using Newtonsoft.Json;

namespace Frontend.Services
{
    public class MatricServices
    {
        HttpClient Http = new HttpClient();

        protected string APIURL = "https://localhost:7192/api/Plant/";
        public async Task<Metric> PlantData(string plantGuid)
        {
            try
            {
                // using to limit the scope of the HttpClient to this method.
                using (HttpClient client = new())
                {
                    // Add the authorization header to the request with the given token.
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Send a GET request to fetch user data from the "GetUsers" endpoint.
                    HttpResponseMessage response = await client.GetAsync(APIURL + $"byGuid?guid={plantGuid}");

                    // Check if the request was successful.
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response content into a list of UserDTO objects.
                        Metric plantData = JsonConvert.DeserializeObject<Metric>(await response.Content.ReadAsStringAsync());

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
    }
}
