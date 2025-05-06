using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserRepo
{
    private readonly PotPalDbContext _dbContext;
    private readonly IConfiguration _configuration;
   
    public UserRepo(PotPalDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<User?> GetByTokenAsync(string token)
    {
        return await _dbContext.Users.FindAsync(token);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<UserDTO> AddAsync(CreateUserDTO userDTO)
    {

        User createUser = new User
        {
            Email = userDTO.Email,
            UserName = userDTO.UserName,
            Password = userDTO.Password,
        };

        await _dbContext.Users.AddAsync(createUser);
        int result = await _dbContext.SaveChangesAsync();

        if (result < 1) 
        {
            throw new InvalidOperationException("Failed to add the user to the database.");
        }

        string Token = GenerateToken(createUser.UserName);

        return new UserDTO
        {
            UserName = userDTO.UserName,
            Token = Token,
        };
    }

    public async Task<UserDTO?> LoginAsync(UserLoginDTO login)
    {
        User user;
        try
        {
            user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password) 
                   ?? throw new InvalidOperationException("User not found.");
        }
        catch (Exception)
        {
            return null;
        }

        if (user == null) return null;

        string Token = GenerateToken(user.Email);

        return new UserDTO
        {
            UserName = user.UserName ?? string.Empty,
            Token = Token,
        };
    }

    private string GenerateToken(string userName)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };

        var token = new JwtSecurityToken(

            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}