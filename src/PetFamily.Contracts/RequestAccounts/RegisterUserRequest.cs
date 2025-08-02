namespace PetFamily.Contracts.RequestAccounts;

public record RegisterUserRequest(string Email, string Password, string UserName);