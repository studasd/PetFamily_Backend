using PetFamily.Application.Abstractions;

namespace PetFamily.Application.AccountManagement.Commands.Login;

public record LoginUserCommand (string Email, string Password) : ICommand;
