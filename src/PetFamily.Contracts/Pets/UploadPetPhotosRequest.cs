using Microsoft.AspNetCore.Http;

namespace PetFamily.Contracts.Pets;

public record UploadPetPhotosRequest(IEnumerable<IFormFile> PhotosUpload);
