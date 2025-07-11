using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record UpdateBankingDetailsRequest(Guid VolunteerId, UpdateBankingDetailsRequestDTO BankingDetailsDTO);