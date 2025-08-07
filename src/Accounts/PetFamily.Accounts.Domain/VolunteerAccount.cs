namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
	// ef core
	private VolunteerAccount() { }

	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public string? Certificates { get; set; }

	public string? Requisite { get; set; }

	public int Experience { get; set; }
}