using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.DTOs;

public class PetDto
{
	public Guid Id { get; set; }
	public Guid VolunteerId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int Position { get; set; }

	public string Color { get; set; }

	public string? HealthInfo { get; set; }

	public string AddressCountry { get; set; }
	public string AddressCity { get; set; }
	public string AddressStreet { get; set; }
	public int AddressHouseNumber { get; set; }
	public string? AddressHouseLiter { get; set; }
	public int AddressApartment { get; set; }

	public decimal Weight { get; set; }

	public decimal Height { get; set; }

	public bool? IsNeutered { get; set; }

	public bool? IsVaccinated { get; set; }

	public DateOnly DateBirth { get; set; } = default;

	public PetHelpStatuses HelpStatus { get; set; }

	public string? BankName { get; set; }
	public string? BankDescription { get; set; }

	public DateTime DateCreated { get; set; }

	public Guid SpeciesId { get; set; }
	public Guid BreedId { get; set; }

	// jsonb
	public string Phones { get; set; } = String.Empty;


	////public string FileStorages { get; set; } = String.Empty;
	// from jsonb
	//public IReadOnlyList<FileStorage> FileStorages => fileStorages;
	//private readonly List<FileStorage> fileStorages = [];

	// from conversion
	public FileStorageDto[] FileStorages { get; set; } = null!;

}

public class FileStorageDto
{
	public string PathToStorage { get; set; }
}