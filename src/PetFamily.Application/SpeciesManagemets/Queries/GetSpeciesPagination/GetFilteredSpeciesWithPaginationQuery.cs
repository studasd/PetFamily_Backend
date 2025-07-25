﻿using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Queries.GetSpeciesPagination;

public record GetFilteredSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;