﻿using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.RequestVolonteers;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.BankingDetails;

public record UpdateBankingDetailsCommand(Guid VolunteerId, IEnumerable<BankingDetailsDTO> BankingDetails) : ICommand;