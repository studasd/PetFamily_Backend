using Microsoft.AspNetCore.Http;

namespace PetFamily.Contracts.RequestPets;

public record UploadPetPhotosRequest(IEnumerable<IFormFile> PhotosUpload);
