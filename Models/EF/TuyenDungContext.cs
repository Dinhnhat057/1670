using Microsoft.EntityFrameworkCore;
using TuyenDungCore.Helpers;

namespace TuyenDungCore.Models.EF
{
    public class TuyenDungContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString("TuyenDungApp");
                options.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>().HasData(new AccountEntity 
            { 
                Id = -1, 
                CreatedDate = DateTime.Now,
                Email = "admin@gmail.com",
                Password = Encryptor.MD5Hash("admin"),
                Role = Enums.Roles.QUANTRIVIEN,
                Status = Enums.Status.Active
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<NhaTuyenDungEntity> NhaTuyenDungs { get; set; }
        public DbSet<TinTuyenDungEntity> TinTuyenDungs { get; set; }
        public DbSet<CandidateEntity> Candidates { get; set; }
        public DbSet<RecruitmentEntity> Recruitments { get; set; }
    }

}
