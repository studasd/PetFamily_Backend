namespace PetFamily.Accounts.Domain;

public class ParticipantAccount
{
	// ef core
	private ParticipantAccount() { }

	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid? FavoritePetId { get; set; }
}