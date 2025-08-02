using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.SocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
	public UpdateSocialNetworksCommandValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));

		RuleForEach(c => c.SocialNetworks)
			.MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Link));
	}
}
