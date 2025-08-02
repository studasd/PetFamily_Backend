using PetFamily.Core.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.Entities;

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