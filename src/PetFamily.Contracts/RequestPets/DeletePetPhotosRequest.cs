namespace PetFamily.Contracts.RequestPets;

public record DeletePetPhotosRequest(IEnumerable<string> PhotosDelete);
