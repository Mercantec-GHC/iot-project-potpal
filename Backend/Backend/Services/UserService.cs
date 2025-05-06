using Models;

public class UserService(UserRepo userRepo)
{



    private readonly UserRepo _userRepo = userRepo;


    public async Task<User?> GetByTokenAsync(string token)
    {
        return await _userRepo.GetByTokenAsync(token);
    }


    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepo.GetAllAsync();
    }


    public async Task<UserDTO> AddAsync(CreateUserDTO user)
    {
        return await _userRepo.AddAsync(user);
    }

    public async Task<UserDTO> LoginAsync(UserLoginDTO login)
    {
        var result = await _userRepo.LoginAsync(login);
        if (result == null)
        {
            throw new InvalidOperationException("Login failed: user not found.");
        }
        return result;
    }




}