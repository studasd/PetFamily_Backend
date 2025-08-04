using Microsoft.EntityFrameworkCore;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.Specieses.Infrastructure;

public class DbTestInitializer
{

	public static async Task InitializeAsync(SpeciesWriteDbContext db)
	{
		if (await db.Species.AnyAsync() == true)
			return;

		var breedCats = new Breed[]
		{
			Breed.Create("Persian").Value,
			Breed.Create("Siamese").Value,
			Breed.Create("British").Value,
			Breed.Create("The Sphinx").Value,
			Breed.Create("Scottish").Value
		};
		var speciesCat = Species.Create("Cats", breedCats).Value;
		await db.Species.AddAsync(speciesCat);

		var catSpeciesId = speciesCat.Id;
		var catBreedId1 = breedCats.FirstOrDefault()!.Id;
		var catBreedId2 = breedCats.LastOrDefault()!.Id;



		var breedDogs = new Breed[]
		{
			Breed.Create("The German Shepherd").Value,
			Breed.Create("French Bulldog").Value,
			Breed.Create("The Beagle").Value,
			Breed.Create("The poodle").Value,
			Breed.Create("The Rottweiler").Value,
			Breed.Create("Yorkshire Terrier").Value
		};
		var speciesDog = Species.Create("Dogs", breedDogs).Value;
		var res = await db.Species.AddAsync(speciesDog);

		var dogSpeciesId = speciesDog.Id;
		var dogBreedId1 = breedDogs.FirstOrDefault()!.Id;
		var dogBreedId2 = breedDogs.LastOrDefault()!.Id;

		await db.SaveChangesAsync();

		speciesDog.AddBreeds(breedDogs);
		speciesCat.AddBreeds(breedCats);

		await db.SaveChangesAsync();
	}
}
