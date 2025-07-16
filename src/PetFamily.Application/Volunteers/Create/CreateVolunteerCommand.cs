using PetFamily.Contracts.DTOs;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
	NameDTO Name, 
	string Email, 
	string Description, 
	int ExperienceYears, 
	string Phone, 
	IEnumerable<BankingDetailsDTO> BankingDetails, 
	IEnumerable<SocialNetworkDTO> SocialNetworks
	);
