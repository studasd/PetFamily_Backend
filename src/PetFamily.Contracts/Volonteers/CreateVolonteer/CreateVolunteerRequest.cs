using PetFamily.Domain.Entities;
using PetFamily.Domain.VolunteerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer;

public record CreateVolunteerRequest
{
	public CreateVolunteerRequest(
		string firstname, 
		string lastname, 
		string surname, 
		string email, 
		string description, 
		int experienceYears, 
		string phone, 
		BankingDetailsDTO? bankingDetails, 
		IEnumerable<SocialNetworkDTO>? socialNetworks)
	{
		this.Firstname = firstname;
		this.Lastname = lastname;
		this.Surname = surname;
		this.Email = email;
		this.Description = description;
		this.ExperienceYears = experienceYears;
		this.Phone = phone;
		this.BankingDetails = bankingDetails;
		this.SocialNetworks = socialNetworks ?? [];
	}

	public string Firstname { get; }
	public string Lastname { get; }
	public string Surname { get; }
	public string Email { get; }
	public string Description { get; }
	public int ExperienceYears { get; }
	public string Phone { get; }

	public BankingDetailsDTO? BankingDetails { get; }
	public IEnumerable<SocialNetworkDTO> SocialNetworks { get; } = [];
}
