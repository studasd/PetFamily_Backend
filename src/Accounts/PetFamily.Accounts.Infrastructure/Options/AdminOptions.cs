namespace PetFamily.Accounts.Infrastructure.Options;

public class AdminOptions
{
	public const string ADMIN = "ADMIN";
	public string UserName { get; init; } = "";
	public string Email { get; init; } = "";
	public string Password { get; init; } = "";
}