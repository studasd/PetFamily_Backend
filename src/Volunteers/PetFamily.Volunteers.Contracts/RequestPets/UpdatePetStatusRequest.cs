using PetFamily.Volunteers.Contracts.Enums;

namespace PetFamily.Volunteers.Contracts.RequestPets;

public record UpdatePetStatusRequest(PetHelpStatuses HelpStatus);