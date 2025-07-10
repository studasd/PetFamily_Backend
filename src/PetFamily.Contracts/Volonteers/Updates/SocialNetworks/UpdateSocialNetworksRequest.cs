using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

public record UpdateSocialNetworksRequest(Guid VolunteerId, UpdateSocialNetworksRequestDTO SocialNetworksDTO);