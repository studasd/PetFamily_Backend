namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
	public const string VOLUNTEER = "Volunteer";

	// ef core
	private VolunteerAccount() { }

	public VolunteerAccount(User user)
	{
		Id = Guid.NewGuid();
		User = user;
		UserId = user.Id;
	}


	public Guid Id { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public string? Certificates { get; set; }

	public string? Requisite { get; set; }

	public int Experience { get; set; }
}