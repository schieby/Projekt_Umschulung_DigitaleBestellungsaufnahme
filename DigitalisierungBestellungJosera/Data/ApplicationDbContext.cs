using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DigitalisierungBestellungJosera.Models;

namespace DigitalisierungBestellungJosera.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DigitalisierungBestellungJosera.Models.Tour> Tour { get; set; } = default!;
        public DbSet<DigitalisierungBestellungJosera.Models.Bestellung> Bestellung { get; set; } = default!;
        public DbSet<DigitalisierungBestellungJosera.Models.Kunde> Kunde { get; set; } = default!;
        public DbSet<DigitalisierungBestellungJosera.Models.Position> Position { get; set; } = default!;
        public DbSet<DigitalisierungBestellungJosera.Models.Produkt> Produkt { get; set; } = default!;
    }
}
