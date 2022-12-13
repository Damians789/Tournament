using Microsoft.EntityFrameworkCore;
using Zawody.Data;

namespace Zawody.Data
{
    public class AutoMigration
    {
        private readonly ApplicationDbContext _context;

        public AutoMigration(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (_context.Database.IsRelational())
                {
                    var pendingMigrations = _context.Database.GetPendingMigrations();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _context.Database.Migrate();
                    }
                }
            }
        }
    }
}
