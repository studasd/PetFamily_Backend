namespace PetFamily.Accounts.Controllers.Requests;

public record RegisterUserRequest(string Email, string Password, string UserName);