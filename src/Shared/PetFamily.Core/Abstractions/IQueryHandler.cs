namespace PetFamily.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
	Task<TResponse> HandleAsync(TQuery query, CancellationToken token);
}
