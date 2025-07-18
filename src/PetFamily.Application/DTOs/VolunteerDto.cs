namespace PetFamily.Application.DTOs;

public class VolunteerDto
{
	public Guid Id { get; init; }

	public string Firstname { get; init; }
	public string Lastname { get; init; }
	public string Surname { get; init; }

	public string? Email { get; init; }

	public string? Description { get; init; }

	public int ExperienceYears { get; init; }

	public string Phone { get; init; }

	public bool IsSoftDeleted { get; set; }
	public DateTime? DateDeletion { get; set; }


	// jsonb
	public BankingDetailsDTO[] BankingDetails { get; init; } = [];

	public SocialNetworkDTO[] SocialNetworks { get; init; } = [];

	public PetDto[] Pets { get; init; } = [];
}

public class BankingDetailsDTO
{
	public string? Name { get; init; }
	public string? Description { get; init; }
}

public class SocialNetworkDTO
{
	public string Name { get; init; }
	public string Link { get; init; }
}
