using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.RequestVolonteers;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDTO> SocialNetworks);