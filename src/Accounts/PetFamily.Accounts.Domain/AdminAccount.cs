using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
	public const string ADMIN = "Admin";

	// ef core
	private AdminAccount() { }

	public AdminAccount(User user, FullName fullName)
	{
		Id = Guid.NewGuid();
		User = user;
		FullName = fullName;
	}

	public Guid Id { get; set; }
	public FullName FullName { get; set; }

	public Guid UserId { get; set; } 
	public User User { get; set; }
}