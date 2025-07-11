using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volonteers.Updates.SocialNetworks;

public class UpdateSocialNetworksValidatorDTO : AbstractValidator<UpdateSocialNetworksRequestDTO>
{
	public UpdateSocialNetworksValidatorDTO()
	{
		RuleForEach(c => c.SocialNetworks)
			.MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Link));
	}
}
