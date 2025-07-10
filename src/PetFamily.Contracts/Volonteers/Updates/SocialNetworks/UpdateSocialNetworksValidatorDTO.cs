using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

public class UpdateSocialNetworksValidatorDTO : AbstractValidator<UpdateSocialNetworksRequestDTO>
{
	public UpdateSocialNetworksValidatorDTO()
	{
		RuleForEach(c => c.SocialNetworks)
			.MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Link));
	}
}
