using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class UserService(UserRepo userRepo)
{
  


    private readonly UserRepo _userRepo = userRepo;


    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepo.GetByIdAsync(id);
    }
   
    
}