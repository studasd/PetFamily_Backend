namespace PetFamily.Contracts.Volonteers;

public record UpdateInfoRequest(Guid VolunteerId, UpdateInfoRequestDTO UpdateInfoDTO);
