namespace PetFamily.Contracts.Pets;

public record CreatePetRequest(Guid VolunteerId, CreatePetRequestDTO CreatePetDto);
