using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.Enums;

namespace PetFamily.Contracts.RequestPets;

public record UpdatePetInfoRequest(
	Guid SpeciesId,
	Guid BreedId,
	string Name, 
	string Description, 
	string Color, 
	string? HealthInfo,
	AddressDTO Address, 
	DateOnly DateBirth,
	decimal Weight,
	decimal Height, 
	IEnumerable<string> Phones, 
	bool? IsNeutered, 
	bool? IsVaccinated, 
	PetHelpStatuses HelpStatus,
	BankingDetailsDTO BankingВetails);