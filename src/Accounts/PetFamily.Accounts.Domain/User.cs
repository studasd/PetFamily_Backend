using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
	private User() { }
	private User(string userName, string email, Role role) 
	{
		UserName = userName;
		Email = email;
		roles = [role];
	}

	public IReadOnlyList<SocialNetwork> SocialNetworks => socialNetworks;
	private readonly List<SocialNetwork> socialNetworks = [];

	public IReadOnlyList<Role> Roles => roles;
	List<Role> roles = [];


	public AdminAccount? AdminAccount { get; private set; }

	public VolunteerAccount? VolunteerAccount { get; private set; }

	public ParticipantAccount? ParticipantAccount { get; private set; }


	public static Result<User, Error> CreateAdmin(string userName, string email, Role role)
	{
		if (role.NormalizedName != AdminAccount.ADMIN)
			return Errors.General.ValueIsInvalid("Role");

		return new User(userName, email, role)
		{
			EmailConfirmed = true
		};
	}


	public static Result<User, Error> CreateParticipant(string userName, string email, Role role)
	{
		if (role.Name != ParticipantAccount.PARTICIPANT)
			return Errors.General.ValueIsInvalid("Role");

		return new User(userName, email, role);
	}


	public static Result<User, Error> CreateVolunteer(string userName, string email, Role role)
	{
		if (role.Name != VolunteerAccount.VOLUNTEER)
			return Errors.General.ValueIsInvalid("Role");

		return new User(userName, email, role);
	}


	public UnitResult<Error> AddSocialNetworks(IEnumerable<SocialNetwork> socNetworks)
	{
		socialNetworks.AddRange(socNetworks);

		return Result.Success<Error>();
	}

	public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
	{
		this.socialNetworks.Clear();
		this.socialNetworks.AddRange(socialNetworks);
	}
}
