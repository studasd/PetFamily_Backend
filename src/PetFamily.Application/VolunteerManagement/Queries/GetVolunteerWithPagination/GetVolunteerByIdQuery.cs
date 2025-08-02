using PetFamily.Core.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetVolunteerWithPagination;

public record GetVolunteerByIdQuery(Guid volunteerId, int Page, int PageSize) : IQuery;
