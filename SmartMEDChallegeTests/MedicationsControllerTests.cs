
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			// No need to get the logs (NullLogger)
			ILogger<MedicationsController> logger = new NullLogger<MedicationsController>();


			// Act & Assert
			try
			{
				// Save values to the database
				using (var context = new MedicationDbContext(options))
				{
					context.Medications.AddRange(
						new Medication { Name = "Aspirin", Quantity = 100, CreationDate = DateTime.UtcNow },
						new Medication { Name = "Ibuprofen", Quantity = 50, CreationDate = DateTime.UtcNow }
					);
					await context.SaveChangesAsync();
				}

				// Perform the test
				using (var context = new MedicationDbContext(options))
				{
					var controller = new MedicationsController(context, logger);
					var result = await controller.GetMedications();

					// Assert
					var okResult = result.Result as OkObjectResult; // Cast to OkObjectResult
					Assert.NotNull(okResult); // Make sure the result is OkObjectResult
					Assert.Equal(200, okResult.StatusCode); // Ensure it's a 200 OK

					var medications = okResult.Value as IEnumerable<Medication>; // Cast Value to the expected type
					Assert.NotNull(medications); // Ensure it's not null
					Assert.Equal(2, medications.Count()); // Check if it has two items
				}
			}
			finally
			{
				// Cleanup: Remove the in-memory database
				using (var context = new MedicationDbContext(options))
				{
					await context.Database.EnsureDeletedAsync();
				}
			}
		}
	}
}