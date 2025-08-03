using Microsoft.AspNetCore.Http;

namespace PetFamily.Volunteers.Presentation.Requests;

public record UploadPetPhotosRequest(IEnumerable<IFormFile> PhotosUpload);
