namespace PetFamily.Accounts.Domain;

public class ParticipantAccount
{
	public const string PARTICIPANT = "Participant";

	// ef core
	private ParticipantAccount() { }

	public ParticipantAccount(User user)
	{
		Id = Guid.NewGuid();
		User = user;
		UserId = user.Id;
	}


	public Guid Id { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid? FavoritePetId { get; set; }
}