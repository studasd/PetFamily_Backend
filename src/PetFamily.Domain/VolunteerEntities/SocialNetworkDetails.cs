using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerEntities;

public record SocialNetworkDetails
{
	private readonly List<SocialNetwork> _socialNetworks = [];

	public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;


	public UnitResult<Error> Add(SocialNetwork? sn)
	{
		if (sn is null)
			return Errors.General.ValueIsRequired("SocialNetwork");

		_socialNetworks.Add(sn);

		return Result.Success<Error>();
	}
}
