using FluentAssertions;
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
	public void Add_Pet_First_Empty_Return_Success_Result()
	{
		// arrange
		// подготовка к тесту
		var volunteer = CreateVolunteerWithPet(0);

		var petId = PetId.NewPeetId();
		var pet = CreatePet(petId);


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


	[Fact]
	public void Add_Pet_First_Other_Return_Success_Result()
	{
		// arrange
		var petCount = 5;
		var volunteer = CreateVolunteerWithPet(petCount);

		var petId = PetId.NewPeetId();
		var petToAdd = CreatePet(petId);


		// act
		// вызов тестируемого метода
		var result = volunteer.AddPet(petToAdd);


		// assert
		// проверка результата
		var addedPet = volunteer.GetPetById(petId);

		var positionLast = Position.Create(petCount + 1);

		Assert.True(result.IsSuccess);
		Assert.True(addedPet.IsSuccess);
		Assert.Equal(addedPet.Value.Id, petToAdd.Id);
		Assert.Equal(addedPet.Value.Position, positionLast);
	}


	[Fact]
	public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
	{
		// arange
		var countPet = 5;

		var volunteer = CreateVolunteerWithPet(countPet);

		var secondPosition = Position.Create(2).Value;

		var firstPet = volunteer.Pets[0];
		var secondPet = volunteer.Pets[1];
		var thirdPet = volunteer.Pets[2];
		var fourthPet = volunteer.Pets[3];
		var fifthPet = volunteer.Pets[4];

		// act
		var result = volunteer.MovePet(secondPet, secondPosition);


		// assert
		result.IsSuccess.Should().BeTrue();
		firstPet.Position.Should().Be(Position.Create(1).Value);
		secondPet.Position.Should().Be(Position.Create(2).Value);
		thirdPet.Position.Should().Be(Position.Create(3).Value);
		fourthPet.Position.Should().Be(Position.Create(4).Value);
		fifthPet.Position.Should().Be(Position.Create(5).Value);
	}


	[Fact]
	public void Move_Pet_Should_Move_Other_Pet_Forward_When_New_Position_Is_Lower()
	{
		// 1 2 3 4 5 old
		// 1 3 4 2 5 new

		// arange
		var countPet = 5;

		var volunteer = CreateVolunteerWithPet(countPet);

		var secondPosition = Position.Create(2).Value;

		var firstPet = volunteer.Pets[0];
		var secondPet = volunteer.Pets[1];
		var thirdPet = volunteer.Pets[2];
		var fourthPet = volunteer.Pets[3];
		var fifthPet = volunteer.Pets[4];

		// act
		var result = volunteer.MovePet(fourthPet, secondPosition);


		// assert
		result.IsSuccess.Should().BeTrue();
		firstPet.Position.Should().Be(Position.Create(1).Value);
		secondPet.Position.Should().Be(Position.Create(3).Value);
		thirdPet.Position.Should().Be(Position.Create(4).Value);
		fourthPet.Position.Should().Be(Position.Create(2).Value);
		fifthPet.Position.Should().Be(Position.Create(5).Value);
	}

	[Fact]
	public void Move_Pet_Should_Move_Other_Pet_Back_When_New_Position_Is_Grater()
	{
		// 1 2 3 4 5 old
		// 1 4 2 3 5 new

		// arange
		var countPet = 5;

		var volunteer = CreateVolunteerWithPet(countPet);

		var fourthPosition = Position.Create(4).Value;

		var firstPet = volunteer.Pets[0];
		var secondPet = volunteer.Pets[1];
		var thirdPet = volunteer.Pets[2];
		var fourthPet = volunteer.Pets[3];
		var fifthPet = volunteer.Pets[4];

		// act
		var result = volunteer.MovePet(secondPet, fourthPosition);


		// assert
		result.IsSuccess.Should().BeTrue();
		firstPet.Position.Should().Be(Position.Create(1).Value);
		secondPet.Position.Should().Be(Position.Create(4).Value);
		thirdPet.Position.Should().Be(Position.Create(2).Value);
		fourthPet.Position.Should().Be(Position.Create(3).Value);
		fifthPet.Position.Should().Be(Position.Create(5).Value);
	}

	[Fact]
	public void Move_Pet_Should_Move_Other_Pet_Forward_When_New_Position_Is_First()
	{
		// 1 2 3 4 5 old
		// 2 3 4 5 1 new

		// arange
		var countPet = 5;

		var volunteer = CreateVolunteerWithPet(countPet);

		var firstPosition = Position.Create(1).Value;

		var firstPet = volunteer.Pets[0];
		var secondPet = volunteer.Pets[1];
		var thirdPet = volunteer.Pets[2];
		var fourthPet = volunteer.Pets[3];
		var fifthPet = volunteer.Pets[4];

		// act
		var result = volunteer.MovePet(fifthPet, firstPosition);


		// assert
		result.IsSuccess.Should().BeTrue();
		firstPet.Position.Should().Be(Position.Create(2).Value);
		secondPet.Position.Should().Be(Position.Create(3).Value);
		thirdPet.Position.Should().Be(Position.Create(4).Value);
		fourthPet.Position.Should().Be(Position.Create(5).Value);
		fifthPet.Position.Should().Be(Position.Create(1).Value);
	}

	[Fact]
	public void Move_Pet_Should_Move_Other_Pet_Back_When_New_Position_Is_Last()
	{
		// 1 2 3 4 5 old
		// 5 1 2 3 4 new

		// arange
		var countPet = 5;

		var volunteer = CreateVolunteerWithPet(countPet);

		var fifthPosition = Position.Create(5).Value;

		var firstPet = volunteer.Pets[0];
		var secondPet = volunteer.Pets[1];
		var thirdPet = volunteer.Pets[2];
		var fourthPet = volunteer.Pets[3];
		var fifthPet = volunteer.Pets[4];

		// act
		var result = volunteer.MovePet(firstPet, fifthPosition);


		// assert
		result.IsSuccess.Should().BeTrue();
		firstPet.Position.Should().Be(Position.Create(5).Value);
		secondPet.Position.Should().Be(Position.Create(1).Value);
		thirdPet.Position.Should().Be(Position.Create(2).Value);
		fourthPet.Position.Should().Be(Position.Create(3).Value);
		fifthPet.Position.Should().Be(Position.Create(4).Value);
	}


	private Volunteer CreateVolunteerWithPet(int countPet)
	{
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


		for(int i=0; i<countPet; i++)
		{
			var pet = CreatePet();

			volunteer.AddPet(pet);
		}

		return volunteer;
	}


	private Pet CreatePet(PetId? petId = null)
	{
		var pet = new Pet(
				id: petId ?? PetId.NewPeetId(),
				name: "nameCat",
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

		return pet;
	}
}