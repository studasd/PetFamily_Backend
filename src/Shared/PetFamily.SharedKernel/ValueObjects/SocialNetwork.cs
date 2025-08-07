using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class SocialNetwork
{
	private SocialNetwork() { }

	private SocialNetwork(string name, string link)
	{
		Name = name;
		Link = link;
	}

	public string Name { get; set; }
	public string Link { get; set; }

	public static Result<SocialNetwork, Error> Create(string name, string link)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsInvalid("Name");

		if (string.IsNullOrWhiteSpace(link))
			return Errors.General.ValueIsInvalid("Link");

		return new SocialNetwork(name, link);
	}
}
