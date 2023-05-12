using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Models.DataContext
{
    public class DataContext: IdentityDbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Header> Header { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobDetail> Jobs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<LostCompany> LostCompanies { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

    }
}
