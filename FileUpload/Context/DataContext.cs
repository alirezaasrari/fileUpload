using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Context
{
    public class DataContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<FileDetails> FileDetails { get; set; }
    }
}
