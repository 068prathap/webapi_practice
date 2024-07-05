using InterviewApiPractice.Data;
using InterviewApiPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        public readonly InterviewPracticeContext context;
        public RegisterController(InterviewPracticeContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route ("/register")]
        public async Task<IActionResult> Register(UserList userList)
        {
            try
            {
                context.Add(userList);
                await context.SaveChangesAsync();
                return StatusCode(201, "user Register succesfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "something went wrong try again");
            }
        }
    }
}
