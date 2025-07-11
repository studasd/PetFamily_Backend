using PetFamily.Contracts.Pets;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Pets.Add;

public record AddPetCommand(
	Guid PetId, 
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
	AddPetRequestAddressDTO AddressDTO
	);
