using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartMEDChallege.Data;
using SmartMEDChallege.DTOs;
using SmartMEDChallege.Interfaces;
using SmartMEDChallege.Models;

namespace SmartMEDChallege.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicationsController : ControllerBase
	{
		private readonly IMedicationService _medicationService;
		private readonly ILogger<MedicationsController> _logger;

		public MedicationsController(IMedicationService medicationService, ILogger<MedicationsController> logger)
		{
			_medicationService = medicationService;
			_logger = logger;
		}

		// GET: api/Medications
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
		{
			try
			{
				var medications = await _medicationService.GetAllMedicationsAsync();
				return Ok(medications); // Return 200 OK with the list of medications
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while fetching medications");
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}

		// POST: api/Medications
		[HttpPost]
		public async Task<ActionResult<Medication>> CreateMedication([FromBody] CreateMedicationDto createMedicationDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Return 400 Bad Request if the model state is invalid
			}
			try
			{
				var medication = await _medicationService.CreateMedicationAsync(createMedicationDto);

				// Return 201 Created with the location of the created resource
				return CreatedAtAction(nameof(GetMedications), new { id = medication.Id }, medication);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a medication");
				return StatusCode(500, "An error occurred while creating the medication.");
			}
		}

		// DELETE: api/Medications/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMedication(Guid id)
		{
			try
			{
				var result = await _medicationService.DeleteMedicationAsync(id);

				if (!result)
				{
					return NotFound(); // Return 404 if the medication is not found
				}

				return NoContent(); // Return 204 No Content on successful deletion
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while deleting medication with id: {id}");
				return StatusCode(500, "An error occurred while deleting the medication.");
			}

		}
	}
}
