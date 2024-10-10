using Microsoft.EntityFrameworkCore;
using SmartMEDChallege.Data;
using SmartMEDChallege.DTOs;
using SmartMEDChallege.Interfaces;
using SmartMEDChallege.Models;

namespace SmartMEDChallege.Services
{
	public class MedicationService : IMedicationService
	{
		private readonly MedicationDbContext _context;

		public MedicationService(MedicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
		{
			return await _context.Medications.ToListAsync();
		}

		public async Task<Medication> CreateMedicationAsync(CreateMedicationDto createMedicationDto)
		{
			var medication = new Medication
			{
				Id = Guid.NewGuid(),
				Name = createMedicationDto.Name,
				Quantity = createMedicationDto.Quantity,
				CreationDate = DateTime.UtcNow
			};

			_context.Medications.Add(medication);
			await _context.SaveChangesAsync();

			return medication;
		}

		public async Task<bool> DeleteMedicationAsync(Guid id)
		{
			var medication = await _context.Medications.FindAsync(id);
			if (medication == null)
			{
				return false;
			}

			_context.Medications.Remove(medication);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
