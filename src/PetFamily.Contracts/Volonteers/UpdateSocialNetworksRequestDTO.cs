using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record UpdateSocialNetworksRequestDTO(IEnumerable<SocialNetworkDTO> SocialNetworks);