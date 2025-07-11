using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record UpdateSocialNetworksRequest(Guid VolunteerId, UpdateSocialNetworksRequestDTO SocialNetworksDTO);