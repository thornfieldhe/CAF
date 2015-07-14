
namespace CAF.FSModels
{
    using System.Data.Entity;

    public class Context : DbContext
    {
        public DbSet<Directory> Directories { get; set; }
    }
}
