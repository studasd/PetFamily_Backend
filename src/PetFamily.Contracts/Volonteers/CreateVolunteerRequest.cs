using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record CreateVolunteerRequest
{
	public CreateVolunteerRequest(
		NameDTO name,
		string email, 
		string description, 
		int experienceYears, 
		string phone, 
		IEnumerable<BankingDetailsDTO>? bankingDetails, 
		IEnumerable<SocialNetworkDTO>? socialNetworks)
	{
		Name = name;
		Email = email;
		Description = description;
		ExperienceYears = experienceYears;
		Phone = phone;
		BankingDetails = bankingDetails ?? [];
		SocialNetworks = socialNetworks ?? [];
	}

	public NameDTO Name { get; }
	public string Email { get; }
	public string Description { get; }
	public int ExperienceYears { get; }
	public string Phone { get; }

	public IEnumerable<BankingDetailsDTO> BankingDetails { get; } = [];
	public IEnumerable<SocialNetworkDTO> SocialNetworks { get; } = [];
}
