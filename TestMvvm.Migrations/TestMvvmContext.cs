using Microsoft.EntityFrameworkCore;
using TestMvvm.Migrations.Configuration;

namespace TestMvvm.Migrations
{
    public partial class TestMvvmContext : DbContext
    {
        public TestMvvmContext(DbContextOptions<TestMvvmContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Configure(builder);
        }


        private static void Configure(ModelBuilder builder)
        {
             builder.ApplyConfiguration(new AircraftConfiguration());

        }
    }
}
