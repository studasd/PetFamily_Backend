using PetFamily.Application.Abstractions;

namespace PetFamily.Application.AccountManagement.Commands.Register;

public record RegisterUserCommand(string Email, string Password, string UserName) : ICommand;