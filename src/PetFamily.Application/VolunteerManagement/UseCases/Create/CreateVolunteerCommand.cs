using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs;

namespace PetFamily.Application.VolunteerManagement.UseCases.Create;

public record CreateVolunteerCommand(
	NameDTO Name, 
	string Email, 
	string Description, 
	int ExperienceYears, 
	string Phone, 
	IEnumerable<BankingDetailsDTO> BankingDetails, 
	IEnumerable<SocialNetworkDTO> SocialNetworks
	) : ICommand;
