namespace PetFamily.Core.Options;

public class RefreshSessionOptions
{
	public const string REFRESHSESSION = "RefreshSession";

	public int ExpiredDaysTime { get; init; }
}
