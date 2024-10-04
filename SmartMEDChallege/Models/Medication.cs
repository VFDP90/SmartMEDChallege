using System.ComponentModel.DataAnnotations;

namespace SmartMEDChallege.Models
{
	public class Medication
	{
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
		public int Quantity { get; set; }

		public DateTime CreationDate { get; set; }
	}
}
