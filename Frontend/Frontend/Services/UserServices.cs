using Models;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace PotPalFrontend.Services
{
    public class UserServices
    {
        // HttpClient is supost to only be instanuated onse rather then per use
        HttpClient Http = new HttpClient();

        protected string APIURL = "https://localhost:7192/api/User/";
        public async Task<UserDTO> LoginAsync(UserLoginDTO login)
        {
            if (login == null) return null;
            //Serialize to JsonData 
            string jsonData = JsonConvert.SerializeObject(login);
            //Provides a Http Content on a string
            StringContent data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //Reprisent a HttpResponse including status code and Conten
            HttpResponseMessage httpResponse;
            try
            {
               httpResponse = await Http.PostAsync(APIURL + "login", data);
            }
            catch (Exception ex)
            {

                throw;
            }
           

            if (httpResponse.IsSuccessStatusCode)
            {
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(await httpResponse.Content.ReadAsStringAsync());
                if (user != null) return user;
            }
            return null;

        }

        public async Task<UserDTO> AddAsync(CreateUserDTO create)
        {
            if (create == null) return null;

            string jsonData = JsonConvert.SerializeObject(create);

            StringContent data = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await Http.PostAsync(APIURL, data);
            }
            catch (Exception ex)
            {

                throw;
            }

            if (httpResponse.IsSuccessStatusCode)
            {
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(await httpResponse.Content.ReadAsStringAsync());
                if (user != null) return user;
            }
            return null;
        }
    }
}
