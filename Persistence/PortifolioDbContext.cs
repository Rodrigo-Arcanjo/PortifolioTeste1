using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

namespace PortifolioTeste1.Persistence
{
    public class PortifolioDbContext : DbContext
    {

        public PortifolioDbContext(DbContextOptions<PortifolioDbContext> opc) : base(opc)
        {

        }

    }
}
