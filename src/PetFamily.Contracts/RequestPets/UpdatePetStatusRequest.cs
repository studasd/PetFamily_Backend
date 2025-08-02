using PetFamily.Contracts.Enums;

namespace PetFamily.Contracts.RequestPets;

public record UpdatePetStatusRequest(PetHelpStatuses HelpStatus);