using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Domain.Entities;

public record PetUpdateInfoDto(
	string Name,
	string Description,
	string Color,
	string? HealthInfo,
	Address Address,
	decimal Weight,
	decimal Height,
	IEnumerable<Phone> Phones,
	bool? IsNeutered,
	bool? IsVaccinated,
	DateOnly DateBirth,
	PetHelpStatuses HelpStatus,
	BankingDetails BankingDetails,
	PetType PetType
);