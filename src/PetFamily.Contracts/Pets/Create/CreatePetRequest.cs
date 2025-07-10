namespace PetFamily.Contracts.Pets.Create;

public record CreatePetRequest(Guid VolunteerId, CreatePetRequestDTO CreatePetDto);
