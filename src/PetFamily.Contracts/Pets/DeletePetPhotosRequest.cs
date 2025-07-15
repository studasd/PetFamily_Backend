namespace PetFamily.Contracts.Pets;

public record DeletePetPhotosRequest(IEnumerable<string> PhotosDelete);
