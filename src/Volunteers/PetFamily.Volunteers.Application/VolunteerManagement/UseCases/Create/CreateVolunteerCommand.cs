using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Create;

public record CreateVolunteerCommand(
	NameDTO Name, 
	string Email, 
	string Description, 
	int ExperienceYears, 
	string Phone, 
	IEnumerable<BankingDetailsDTO> BankingDetails
	) : ICommand;
