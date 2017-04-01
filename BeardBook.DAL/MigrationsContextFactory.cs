using System.Data.Entity.Infrastructure;

namespace BeardBook.DAL
{
    public class MigrationsContextFactory : IDbContextFactory<BeardBookDbContext>
    {
        public BeardBookDbContext Create()
        {
            return new BeardBookDbContext("DefaultConnection");
        }
    }
}
