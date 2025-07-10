using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Shared.ValueObjects;

public record SocialNetwork(string Name, string Link)
{
	public static Result<SocialNetwork, Error> Create(string name, string link)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsInvalid("Name");

		if (string.IsNullOrWhiteSpace(link))
			return Errors.General.ValueIsInvalid("Link");

		return new SocialNetwork(name, link);
	}
}
