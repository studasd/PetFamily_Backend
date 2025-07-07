using FluentValidation;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer.Validators;

public class SocialNetworkDTOValidator : AbstractValidator<SocialNetworkDTO>
{
	public SocialNetworkDTOValidator()
	{
		RuleFor(c => c.Name)
			.NotEmpty().WithErrorCode("socnetwork_name_invalid").WithMessage("SocNetwork name is not empty")
			.MaximumLength(50).WithErrorCode("socnetwork_name_invalid").WithMessage("SocNetwork name maximum lenght: 50");

		RuleFor(c => c.Link)
			.NotEmpty().WithErrorCode("socnetwork_link_invalid").WithMessage("SocNetwork link is not empty")
			.MaximumLength(100).WithErrorCode("socnetwork_link_invalid").WithMessage("SocNetwork link maximum lenght: 100");
	}
}