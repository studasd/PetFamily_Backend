using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.RequestVolonteers;

namespace PetFamily.Application.Volonteers.Updates.SocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDTO> SocialNetworks);
