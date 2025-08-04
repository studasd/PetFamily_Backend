namespace PetFamily.Volunteers.Contracts.RequestPets;

public record DeletePetPhotosRequest(IEnumerable<string> PhotosDelete);
