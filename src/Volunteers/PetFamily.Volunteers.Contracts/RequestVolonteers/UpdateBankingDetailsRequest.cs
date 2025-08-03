using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.RequestVolonteers;

public record UpdateBankingDetailsRequest(IEnumerable<BankingDetailsDTO> BankingDetails);