using InterviewApiPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Reflection;
using System.Reflection.PortableExecutable;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoController : ControllerBase
    {
        public readonly IConfiguration Configuration;
        public AdoController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: api/<AdoController>
        [HttpGet]
        public IEnumerable<UserList> Get()
        {
            string connectionstring = Configuration.GetConnectionString("ConnectionString1");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = new SqlCommand("select * from userList",connection);
            List<UserList> userList = new List<UserList>();
            using(SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserList user = new UserList
                    {
                        name = reader.GetString(reader.GetOrdinal("name")),
                        age = reader.GetInt32(reader.GetOrdinal("age")),
                        gender = reader.GetString(reader.GetOrdinal("gender"))
                        // Set other properties similarly
                    };
                    userList.Add(user);
                }
            }
            connection.Close();
            return userList;
        }

        // POST api/<AdoController>
        [HttpPost]
        public void Post(UserList userList)
        {
            string connectionstring = Configuration.GetConnectionString("ConnectionString1");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO userList (name, age, gender)" + "VALUES (@name, @age, @gender)", connection);
            cmd.Parameters.AddWithValue("@name", userList.name);
            cmd.Parameters.AddWithValue("@age", userList.age);
            cmd.Parameters.AddWithValue("@gender", userList.gender);

            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
