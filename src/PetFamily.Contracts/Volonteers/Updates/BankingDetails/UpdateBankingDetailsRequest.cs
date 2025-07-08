using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Updates.BankingDetails;

public record UpdateBankingDetailsRequest(Guid VolunteerId, UpdateBankingDetailsRequestDTO BankingDetailsDTO);