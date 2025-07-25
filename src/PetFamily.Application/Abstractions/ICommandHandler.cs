﻿
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
	Task<Result<TResponse, ErrorList>> HandleAsync(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
	Task<UnitResult<ErrorList>> HandleAsync(TCommand command, CancellationToken token);
}
