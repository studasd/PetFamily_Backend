using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Contracts.RequestPets;

public record UpdatePetStatusRequest(PetHelpStatuses HelpStatus);