using SmartMEDChallege.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartMEDChallege.Data
{
	public class MedicationDbContext : DbContext
	{
		public MedicationDbContext(DbContextOptions<MedicationDbContext> options)
		: base(options)
		{
		}

		public DbSet<Medication> Medications { get; set; }
	}
}
