using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.UnitTests;

public class VolunteerTests
{
	[Fact]
	public void Add_Pet_First_Return_Success_Result()
	{
		// arrange
		// подготовка к тесту
		var volunteer = new Volunteer(
			id: VolunteerId.NewVolunteerId(),
			name: VolunteerName.Create(
				firstname: "firstname",
				lastname: "lastname",
				surname: "surname").Value,
			email: "qwerty@uio.pa",
			description: "description",
			experienceYears: 1,
			phone: Phone.Create(phone: "87897897977").Value
			);

		var petId = PetId.NewPeetId();
		var pet = new Pet(
			id: petId,
			name: "nameCat",
			type: PetTypes.Cat,
			description: "description",
			color: "color",
			weight: 3,
			height: 5,
			phones: [Phone.Create("79879879879").Value],
			helpStatus: PetHelpStatuses.NeedsHelp,
			address: Address.Create(
				country: "country",
				city: "city",
				street: "street",
				houseNumber: 88,
				apartment: 22,
				houseLiter: "houseLiter"
				).Value,
			petType: PetType.Create(breedId: BreedId.NewBreedId(), speciesId: SpeciesId.NewSpeciesId()).Value
			);

		// act
		// вызов тестируемого метода
		var result = volunteer.AddPet(pet);

		// assert
		// проверка результата
		var addedPet = volunteer.GetPetById(petId);

		Assert.True(result.IsSuccess);
		Assert.True(addedPet.IsSuccess);
		Assert.Equal(addedPet.Value.Id, pet.Id);
		Assert.Equal(addedPet.Value.Position, Position.First);
	}
}