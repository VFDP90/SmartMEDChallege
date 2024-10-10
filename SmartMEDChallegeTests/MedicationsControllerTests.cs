using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SmartMEDChallege.Controllers;
using SmartMEDChallege.Interfaces;
using SmartMEDChallege.Models;

namespace SmartMEDChallegeTests
{
	public class MedicationsControllerTests
	{
		private readonly MedicationsController _controller;
		private readonly Mock<IMedicationService> _mockMedicationService;
		private readonly Mock<ILogger<MedicationsController>> _mockLogger;


		public MedicationsControllerTests()
		{
			// Mock the medication service
			_mockMedicationService = new Mock<IMedicationService>();

			// Mock logger (optional)
			_mockLogger = new Mock<ILogger<MedicationsController>>();

			// Initialize the controller with the mocked service
			_controller = new MedicationsController(_mockMedicationService.Object, _mockLogger.Object);
		}

		[Fact]
		public async Task GetMedications_ReturnsOkResult_WithListOfMedications()
		{
			// Arrange
			var medications = new List<Medication>
			{
				new Medication { Id = Guid.NewGuid(), Name = "Medication A", Quantity = 10 },
				new Medication { Id = Guid.NewGuid(), Name = "Medication B", Quantity = 5 }
			};

			// Setup the mock service to return the list of medications
			_mockMedicationService.Setup(service => service.GetAllMedicationsAsync())
								  .ReturnsAsync(medications);
			try
			{

				// Act
				var result = await _controller.GetMedications();

				// Assert
				var okResult = Assert.IsType<OkObjectResult>(result.Result); // Assert we got 200 OK
				var resultValue = okResult.Value as IEnumerable<Medication>; // Cast Value to the expected type
				Assert.NotNull(resultValue); // Ensure it's not null
				Assert.Equal(2, resultValue.Count()); // Ensure the correct number of medications were returned

			}
			finally 
			{
				// no need to clean up
			}
		}
	}
}