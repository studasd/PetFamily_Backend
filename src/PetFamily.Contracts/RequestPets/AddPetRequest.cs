using Microsoft.AspNetCore.Http;
using PetFamily.Contracts.DTOs;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Contracts.RequestPets;

public record AddPetRequest(
	string Name,
	string Description,
	Guid BreedId,
	Guid SpeciesId,
	string Color,
	decimal Weight,
	decimal Height,
	string Phone,
	PetHelpStatuses HelpStatus,
	AddressDTO AddressDTO);
