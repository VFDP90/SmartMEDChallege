using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		public MedicationsController(MedicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Medications
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
		{
			var medications = await _context.Medications.ToListAsync();
			return Ok(medications); // Return 200 OK with the list of medications
		}

		// POST: api/Medications
		[HttpPost]
		public async Task<ActionResult<Medication>> CreateMedication([FromBody] CreateMedicationDto createMedicationDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Return 400 Bad Request if the model state is invalid
			}

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

		// DELETE: api/Medications/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMedication(Guid id)
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
	}
}
