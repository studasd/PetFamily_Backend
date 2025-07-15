using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.RequestVolonteers;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDTO> SocialNetworks);