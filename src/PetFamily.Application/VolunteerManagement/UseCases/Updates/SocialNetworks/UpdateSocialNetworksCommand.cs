using PetFamily.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.SocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDTO> SocialNetworks) : ICommand;
