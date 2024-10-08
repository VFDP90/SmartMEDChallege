using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartMEDChallege.Data;
using SmartMEDChallege.DTOs;
using SmartMEDChallege.Models;

namespace SmartMEDChallege.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicationsController : ControllerBase
	{
		private readonly MedicationDbContext _context;
		private readonly ILogger<MedicationsController> _logger;

		public MedicationsController(MedicationDbContext context, ILogger<MedicationsController> logger)
		{
			_context = context;
			_logger = logger;
		}

		// GET: api/Medications
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
		{
			try
			{
				var medications = await _context.Medications.ToListAsync();
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
				// Map the DTO to the Medication entity
				var medication = new Medication
				{
					Id = Guid.NewGuid(),
					Name = createMedicationDto.Name,
					Quantity = createMedicationDto.Quantity,
					CreationDate = DateTime.UtcNow
				};

				_context.Medications.Add(medication);
				await _context.SaveChangesAsync();

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
				var medication = await _context.Medications.FindAsync(id);

				if (medication == null)
				{
					return NotFound(); // Return 404 if the medication is not found
				}

				_context.Medications.Remove(medication);
				await _context.SaveChangesAsync();

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
