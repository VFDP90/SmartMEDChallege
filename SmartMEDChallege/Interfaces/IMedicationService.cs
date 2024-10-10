using SmartMEDChallege.DTOs;
using SmartMEDChallege.Models;

namespace SmartMEDChallege.Interfaces
{
	public interface IMedicationService
	{
		Task<IEnumerable<Medication>> GetAllMedicationsAsync();
		Task<Medication> CreateMedicationAsync(CreateMedicationDto createMedicationDto);
		Task<bool> DeleteMedicationAsync(Guid id);
	}
}
