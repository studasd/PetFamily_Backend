using PetFamily.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.BankingDetails;

public record UpdateBankingDetailsCommand(Guid VolunteerId, IEnumerable<BankingDetailsDTO> BankingDetails) : ICommand;