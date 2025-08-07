using PetFamily.Core.DTOs;

namespace PetFamily.Accounts.Contracts.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDTO> SocialNetworks);
