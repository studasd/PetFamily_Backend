using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Contracts.Pets;

public record CreatePetRequestDTO(
	string Name,
	PetTypes Type,
	string Description,
	string Breed,
	string Species,
	string Color,
	decimal Weight,
	decimal Height,
	string Phone,
	PetHelpStatuses HelpStatus,
	CreatePetRequestAddressDTO AddressDTO);
