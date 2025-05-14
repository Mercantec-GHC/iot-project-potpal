using Models;

namespace PotPalFrontend.Services
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;

        public UserServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO?> LoginAsync(UserLoginDTO login)
        {
            Console.WriteLine($"🌐 Sending POST to: {_httpClient.BaseAddress}api/user/login");


            var response = await _httpClient.PostAsJsonAsync("api/user/login", login);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDTO>();

            return null;
        }

        public async Task<UserDTO?> AddAsync(CreateUserDTO user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/register", user);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDTO>();

            return null;
        }
    }
}
