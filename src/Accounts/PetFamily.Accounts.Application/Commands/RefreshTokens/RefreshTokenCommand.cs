using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokenCommand(string AccessToken, Guid RefreshToken) : ICommand;
