using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.RequestVolonteers;

namespace PetFamily.Application.Volunteers.Updates.SocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDTO> SocialNetworks);
