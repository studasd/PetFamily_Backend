using Microsoft.EntityFrameworkCore;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure;

public class DbTestInitializer
{

	public static async Task InitializeAsync(WriteDbContext db)
	{
		if (await db.Volunteers.AnyAsync() == true)
			return;

		//var breedCats = new Breed[] 
		//{ 
		//	Breed.Create("Persian").Value,
		//	Breed.Create("Siamese").Value,
		//	Breed.Create("British").Value,
		//	Breed.Create("The Sphinx").Value,
		//	Breed.Create("Scottish").Value
		//};
		//var speciesCat = Species.Create("Cats", breedCats).Value;
		//await db.Species.AddAsync(speciesCat);
		
		//var catSpeciesId = speciesCat.Id;
		//var catBreedId1 = breedCats.FirstOrDefault()!.Id;
		//var catBreedId2 = breedCats.LastOrDefault()!.Id;



		//var breedDogs = new Breed[]
		//{
		//	Breed.Create("The German Shepherd").Value,
		//	Breed.Create("French Bulldog").Value,
		//	Breed.Create("The Beagle").Value,
		//	Breed.Create("The poodle").Value,
		//	Breed.Create("The Rottweiler").Value,
		//	Breed.Create("Yorkshire Terrier").Value
		//};
		//var speciesDog = Species.Create("Dogs", breedDogs).Value;
		//var res = await db.Species.AddAsync(speciesDog);

		//var dogSpeciesId = speciesDog.Id;
		//var dogBreedId1 = breedDogs.FirstOrDefault()!.Id;
		//var dogBreedId2 = breedDogs.LastOrDefault()!.Id;

		//await db.SaveChangesAsync();

		//speciesDog.AddBreeds(breedDogs);
		//speciesCat.AddBreeds(breedCats);

		//await db.SaveChangesAsync();


		//var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		//var newVolunteer = Volunteer.Create(
		//	volunteerName: VolunteerName.Create(
		//		firstname: guid.Substring(0, 5),
		//		lastname: guid.Substring(5, 5),
		//		surname: guid.Substring(10, 5)).Value,
		//	email: $"{guid.Substring(4, 4)}@{guid.Substring(8, 3)}.{guid.Substring(10, 2)}",
		//	description: guid.Substring(5, 15),
		//	experienceYears: Random.Shared.Next(10),
		//	phone: Phone.Create($"{Random.Shared.NextInt64(79014445865, 79994445865)}").Value
		//	).Value;

		//newVolunteer.AddSocialNetworks([SocialNetwork.Create(guid.Substring(5, 7), guid.Substring(8, 17)).Value]);
		//newVolunteer.AddBankingDetails([BankingDetails.Create(guid.Substring(2, 5), guid.Substring(3, 10)).Value]);

		//for (int i = 0; i < 3; i++)
		//{
		//	guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		//	var breedId = i switch
		//	{
		//		0 => catBreedId1,
		//		1 => dogBreedId1,
		//		2 => catBreedId2,
		//	};
		//	var speciesId = i switch
		//	{
		//		0 => catSpeciesId,
		//		1 => dogSpeciesId,
		//		2 => catSpeciesId
		//	};

		//	var pet = Pet.Create(
		//		name: guid.Substring(0, 5),
		//		description: guid.Substring(5, 15),
		//		color: guid.Substring(5, 5),
		//		weight: Math.Round((decimal)Random.Shared.NextDouble() * Random.Shared.Next(1, 20), 2),
		//		height: Math.Round((decimal)Random.Shared.NextDouble() * Random.Shared.Next(1, 20), 2),
		//		phone: Phone.Create($"{Random.Shared.NextInt64(79014445865, 79994445865)}").Value,
		//		helpStatus: PetHelpStatuses.NeedsHelp,
		//		address: Address.Create(
		//			country: "Russia",
		//			city: "Tobok",
		//			street: "Mira",
		//			houseNumber: 5,
		//			apartment: 45).Value,
		//		petType: PetType.Create(
		//			breedId: breedId, 
		//			speciesId: speciesId
		//			).Value
		//		).Value;

		//	newVolunteer.AddPet(pet);
		//}

		//await db.Volunteers.AddAsync(newVolunteer);

		//await db.SaveChangesAsync();
	}
}