using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

public record UpdateSocialNetworksRequestDTO(IEnumerable<SocialNetworkDTO> SocialNetworks);