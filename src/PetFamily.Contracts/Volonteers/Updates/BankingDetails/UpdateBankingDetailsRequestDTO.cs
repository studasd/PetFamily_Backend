using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Updates.BankingDetails;

public record UpdateBankingDetailsRequestDTO(IEnumerable<BankingDetailsDTO> BankingDetails);