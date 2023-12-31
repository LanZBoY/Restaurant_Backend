

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Models;


[ApiController]
public class AuthController(RestaurantContext restaurantContext, IConfiguration configuration) : ControllerBase{
    private readonly RestaurantContext _context = restaurantContext;
    private readonly DbSet<UserModel> UserTable = restaurantContext.Users;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("Register")]
    public ActionResult<UserRegisterDTO> RegisterUser(UserRegisterDTO userdata){
        // check whether username has exist in database
        if (UserTable.Any((item) => item.UserName == userdata.UserName)){
            return BadRequest(new{
                ErrorMessage = "UserName has exist!"
            });
        }
        // start insert data into database
        UserTable.Add(new UserModel{
            UserName = userdata.UserName,
            Password = userdata.Password,
            Mail = userdata.Mail,
            Role = "User"
        });
        // excute SQL
        if(_context.SaveChanges() <= 0){
            return BadRequest(new{
                ErrorMessage = "Data not change"
            });
        }

        return Created();
    }
    
    [HttpPost("Login")]
    public ActionResult Login(UserLoginDTO user){
        // 資料庫搜尋邏輯
        UserModel? findResult = UserTable.Where((item) => (item.UserName == user.UserName) & (item.Password == user.Password)).FirstOrDefault();
        
        if (findResult == null) return NotFound();
        
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration["JwtSetting:Secret"];
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(new Claim[]{
                new(ClaimTypes.Name, findResult.Id.ToString()),
                new(ClaimTypes.Role, findResult.Role),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var tokenString = jwtTokenHandler.WriteToken(token);
        return Ok(new UserRole{Token = tokenString, Role = findResult.Role});
    }

}