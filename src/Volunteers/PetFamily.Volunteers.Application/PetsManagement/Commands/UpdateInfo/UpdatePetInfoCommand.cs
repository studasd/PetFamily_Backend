using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateInfo;

public record UpdatePetInfoCommand(
	Guid VolunteerId,
	Guid PetId,
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
	BankingDetailsDTO BankingВetails) : ICommand;