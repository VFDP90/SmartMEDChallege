using Microsoft.EntityFrameworkCore;
using SmartMEDChallege.Data;
using SmartMEDChallege.Models;
using SmartMEDChallege.Services;

namespace SmartMEDChallegeTests
{
	public class MedicationServiceTests
	{
		private readonly MedicationService _medicationService;
		private readonly MedicationDbContext _context;

		public MedicationServiceTests()
		{
			var options = new DbContextOptionsBuilder<MedicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new MedicationDbContext(options);
			_medicationService = new MedicationService(_context);

		}
		// No need in this case because is In Memory DB
		private void ClearDatabase()
		{
			// Clears the medications table after each test
			_context.Medications.RemoveRange(_context.Medications);
			_context.SaveChanges();
		}

		[Fact]
		public async Task GetAllMedicationsAsync_ReturnsAllMedications()
		{
			try
			{
				// Seed database for this specific test
				_context.Medications.AddRange(
					new Medication { Id = Guid.NewGuid(), Name = "Medication A", Quantity = 10 },
					new Medication { Id = Guid.NewGuid(), Name = "Medication B", Quantity = 5 }
				);
				await _context.SaveChangesAsync(); // Save changes before testing

				// Act
				var medications = await _medicationService.GetAllMedicationsAsync();

				// Assert
				Assert.Equal(2, medications.Count()); // Ensure two records are returned
			}
			finally
			{
				// Ensure the database is cleared after the test
				ClearDatabase();
			}
		}
	}
}
