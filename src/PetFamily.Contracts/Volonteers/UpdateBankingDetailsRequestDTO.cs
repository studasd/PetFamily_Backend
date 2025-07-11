using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record UpdateBankingDetailsRequestDTO(IEnumerable<BankingDetailsDTO> BankingDetails);