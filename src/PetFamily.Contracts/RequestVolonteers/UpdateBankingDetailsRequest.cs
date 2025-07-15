using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.RequestVolonteers;

public record UpdateBankingDetailsRequest(IEnumerable<BankingDetailsDTO> BankingDetails);