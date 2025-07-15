namespace PetFamily.Application.Pets.DeletePhotos;

public record DeletePhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> DeleteFiles);
