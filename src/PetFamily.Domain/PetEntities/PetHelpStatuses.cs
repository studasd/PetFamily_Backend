namespace PetFamily.Domain.PetEntities;

public enum PetHelpStatuses
{
	/// <summary>
	/// Default
	/// </summary>
	None,
	/// <summary>
	/// Нуждается в помощи
	/// </summary>
	NeedsHelp,
	/// <summary>
	/// Ищет дом
	/// </summary>
	LookingHome,
	/// <summary>
	/// Нашел дом
	/// </summary>
	FoundHouse,
}