﻿using Microsoft.EntityFrameworkCore;
using PetFamily.Application.DTOs;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
	IQueryable<VolunteerDto> Volunteers { get; }
	IQueryable<PetDto> Pets { get; }
	IQueryable<SpeciesDto> Species { get; }
	IQueryable<BreedDto> Breeds { get; }
}
