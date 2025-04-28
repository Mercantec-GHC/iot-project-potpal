using Database;
using Microsoft.EntityFrameworkCore;
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
        return await _userRepo.LoginAsync(login);
    }
        



}