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


	// jsonb
	public string BankingDetails { get; init; } = String.Empty;

	public string SocialNetworks { get; init; } = String.Empty;

	public PetDto[] Pets { get; init; } = [];
}
