
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMEDChallege.Controllers;
using SmartMEDChallege.Data;
using SmartMEDChallege.Models;

namespace SmartMEDChallegeTests
{
	public class MedicationsControllerTests
	{
		[Fact]
		public async Task GetMedications_ReturnsAllMedications()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<MedicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestMedicationDb")
				.Options;

			// Seed the database
			using (var context = new MedicationDbContext(options))
			{
				context.Medications.Add(new Medication { Name = "Aspirin", Quantity = 100, CreationDate = DateTime.UtcNow });
				context.Medications.Add(new Medication { Name = "Ibuprofen", Quantity = 50, CreationDate = DateTime.UtcNow });
				context.SaveChanges();
			}

			// Act
			IEnumerable<Medication> medications;
			using (var context = new MedicationDbContext(options))
			{
				var controller = new MedicationsController(context);
				var result = await controller.GetMedications();

				// Assert
				Assert.NotNull(result.Value); // Assert that the result is not null
				medications = result.Value;

			}

			// Final assertion outside the using statement for clarity
			Assert.Equal(2, medications.Count());
		}
	}
}