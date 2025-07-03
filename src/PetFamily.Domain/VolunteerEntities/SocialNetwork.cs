using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerEntities;

public record SocialNetwork(string Name, string Link)
{
	public static Result<SocialNetwork> Create(string name, string link)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<SocialNetwork>("Name social network cannot be empty");

		if (string.IsNullOrWhiteSpace(link))
			return Result.Failure<SocialNetwork>("Link social network cannot be empty");

		return new SocialNetwork(name, link);
	}
}
