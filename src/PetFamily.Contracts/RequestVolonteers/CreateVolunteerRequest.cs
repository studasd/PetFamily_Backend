using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.RequestVolonteers;

public record CreateVolunteerRequest
(
	NameDTO Name,
	string Email,
	string Description,
	int ExperienceYears,
	string Phone,
	IEnumerable<BankingDetailsDTO>? BankingDetails,
	IEnumerable<SocialNetworkDTO>? SocialNetworks
);
