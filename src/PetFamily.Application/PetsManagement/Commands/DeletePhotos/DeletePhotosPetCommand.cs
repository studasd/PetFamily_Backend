namespace PetFamily.Application.PetsManagement.Commands.DeletePhotos;

public record DeletePhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> DeleteFiles);
