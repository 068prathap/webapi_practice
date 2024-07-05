using InterviewApiPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewApiPractice.Data
{
    public class InterviewPracticeContext : DbContext
    {
        public InterviewPracticeContext(DbContextOptions<InterviewPracticeContext> options):base(options)
        {
        }

        public DbSet<UserList> UserList { get; set; }
    }
}