using PetFamily.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Updates.BankingDetailes;

public record UpdateBankingDetailsCommand(Guid VolunteerId, IEnumerable<BankingDetailsDTO> BankingDetails) : ICommand;