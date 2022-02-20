using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDatabaseMaintenance
{
    public class Airport
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(4)]
        public string AirportICAOCode { get; set; }
        [StringLength(3)]
        public string AirportIATACode { get; set; }
        public int Minima { get; set; }
    }

    public class AirportDbContext : DbContext
    {
        public DbSet<Airport> Airports { get; set; }
    }
}

