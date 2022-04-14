using Microsoft.EntityFrameworkCore;
using TestEng.Data.Models;

namespace TestEng.Entities
{
    public class TestEngContext : DbContext
    {
        public TestEngContext (DbContextOptions<TestEngContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
    }
}
