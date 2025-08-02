namespace PetFamily.Accounts.Contracts.Requests;

public record RegisterUserRequest(string Email, string Password, string UserName);