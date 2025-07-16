using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.RequestVolonteers;

namespace PetFamily.Application.Volunteers.Updates.BankingDetails;

public record UpdateBankingDetailsCommand(Guid VolunteerId, IEnumerable<BankingDetailsDTO> BankingDetails);