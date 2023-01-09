using Microsoft.EntityFrameworkCore;

namespace UploadPicture.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ImageEntity> Images { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

    }
}
