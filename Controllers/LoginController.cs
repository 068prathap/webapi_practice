using InterviewApiPractice.Data;
using InterviewApiPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InterviewApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly InterviewPracticeContext context;
        public readonly IConfiguration config;
        public LoginController(InterviewPracticeContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        [HttpPost]
        [Route ("/login")]
        public async Task<IActionResult> login(loginDetails loginDetails)
        {
            try
            {
                //List<String> result = context.Database.SqlQueryRaw<String>($"select * from userList where name={loginDetails.name} AND password={loginDetails.password}").ToList();
                //Console.WriteLine(result.Count);
                var nameParam = new SqlParameter("name", loginDetails.name);
                var passwordParam = new SqlParameter("password", loginDetails.password);

                var result = context.UserList
                    .FromSqlRaw("SELECT * FROM userList WHERE name = @name AND password = @password", nameParam, passwordParam)
                    .FirstOrDefault();

                Console.WriteLine(result);
                if (result!=null)
                {
                    int userId=FindUserId(loginDetails.name, loginDetails.password);
                    string token = TokenGendrator(userId);
                    return Ok(new { userId,token });
                }
                else
                {
                    return NotFound("name or password is wrong");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "something went wrong try again");
            }
        }

        private bool checkUser(string name, string password)
        {
            return context.UserList.Any(a => a.name == name && a.password == password);
        }

        private int FindUserId(string name, string password)
        {
            var user=context.UserList.FirstOrDefault(a=> a.name== name && a.password == password);
            if (user!=null)
            {
                return user.Id;
            }
            return 0;
        }

        private string TokenGendrator(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
            };

            var Sectoken = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}