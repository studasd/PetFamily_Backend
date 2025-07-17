using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.DTOs;

public class PetDto
{
	public Guid Id { get; init; }
	public Guid VolunteerId { get; init; }

	public string Name { get; init; }

	public PetTypes Type { get; init; }

	public string Description { get; init; }

	public int Position { get; init; }

	public string Color { get; init; }

	public string? HealthInfo { get; init; }

	public string AddressCountry { get; init; }
	public string AddressCity { get; init; }
	public string AddressStreet { get; init; }
	public int AddressHouseNumber { get; init; }
	public string? AddressHouseLiter { get; init; }
	public int AddressApartment { get; init; }

	public decimal Weight { get; init; }

	public decimal Height { get; init; }

	public bool? IsNeutered { get; init; }

	public bool? IsVaccinated { get; init; }

	public DateOnly DateBirth { get; init; } = default;

	public PetHelpStatuses HelpStatus { get; init; }

	public string? BankName { get; init; }
	public string? BankDescription { get; init; }

	public DateTime DateCreated { get; init; }

	public Guid SpeciesId { get; init; }
	public Guid BreedId { get; init; }

	// jsonb
	public string Phones { get; init; } = String.Empty;


	////public string FileStorages { get; init; } = String.Empty;
	// from jsonb
	//public IReadOnlyList<FileStorage> FileStorages => fileStorages;
	//private readonly List<FileStorage> fileStorages = [];

	// from conversion
	public FileStorageDto[] FileStorages { get; init; } = null!;

}

public class FileStorageDto
{
	public string PathToStorage { get; set; }
}