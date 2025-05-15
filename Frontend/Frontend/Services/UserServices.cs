using Models;
using Newtonsoft.Json;
using System.Text;

namespace PotPalFrontend.Services
{
    public class UserServices
    {
        private readonly HttpClient _http;

        public UserServices(HttpClient http)
        {
            _http = http;
        }

        public async Task<UserDTO?> LoginAsync(UserLoginDTO login)
        {
            if (login == null) return null;

            string jsonData = JsonConvert.SerializeObject(login);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _http.PostAsync("api/User/login", content);

                Console.WriteLine($"📥 Login response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserDTO>(responseData);
                    return user;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error during login: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
            }

            return null;
        }

        public async Task<UserDTO?> AddAsync(CreateUserDTO create)
        {
            if (create == null) return null;

            string jsonData = JsonConvert.SerializeObject(create);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _http.PostAsync("api/User", content);

                Console.WriteLine($"📥 Register response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserDTO>(responseData);
                    return user;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error during registration: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
            }

            return null;
        }
    }
}
