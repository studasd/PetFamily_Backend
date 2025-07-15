namespace PetFamily.Contracts.Pets;

public record AddPetRequest(Guid VolunteerId, AddPetRequestDTO CreatePetDto);
