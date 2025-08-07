using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;

namespace PetFamily.Accounts.Application.AccountManagement.UseCases.Updates.SocialNetworks;

public record UpdateSocialNetworksCommand(Guid UserId, IEnumerable<SocialNetworkDTO> SocialNetworks) : ICommand;
