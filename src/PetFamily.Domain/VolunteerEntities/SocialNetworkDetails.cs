using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerEntities;

public record SocialNetworkDetails
{
	private readonly List<SocialNetwork> _socialNetworks = [];

	public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;


	public Result Add(SocialNetwork? sn)
	{
		if (sn is null)
			return Result.Failure("SocialNetwork cannot be null to add");

		_socialNetworks.Add(sn);

		return Result.Success();
	}
}
