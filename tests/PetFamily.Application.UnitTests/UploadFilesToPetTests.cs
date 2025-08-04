using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Domain.IDs;
using PetFamily.Volunteers.Application.PetsManagement.Commands.UploadPhotos;
using PetFamily.Volunteers.Application.VolunteerManagement;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;
using System.Data;

namespace PetFamily.Application.UnitTests;

public class UploadFilesToPetTests
{
	private readonly Mock<IFileProvider> fileProviderMock = new();
	private readonly Mock<IVolunteerRepository> volunteerRepositoryMock = new();
	private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
	private readonly Mock<IValidator<UploadPhotosPetCommand>> validatorMock = new();
	private readonly Mock<IMessageQueue<IEnumerable<FileInform>>> messageQueueMock = new();
	private readonly Mock<ILogger<UploadPhotosPetHandler>> loggerMock = new();


	private static Pet GeneratePet() => new Pet(
			id: PetId.NewPeetId(),
			name: "rgeeed",
			description: "dsagvfsdgvsd",
			color: "iohioihohio",
			weight: 1,
			height: 2,
			phones: [Phone.Create("74878778").Value],
			helpStatus: PetHelpStatuses.NeedsHelp,
			address: Address.Create(
				country: "Russia",
				city: "Tobok",
				street: "Mira",
				houseNumber: 5,
				apartment: 45
			).Value,
			petType: new PetType(BreedId.NewBreedId(), SpeciesId.NewSpeciesId())
		);

	private static Volunteer GenerateVolunteer() => new Volunteer(
			id: VolunteerId.NewVolunteerId(),
			name: VolunteerName.Create(
				firstname: "fghfd",
				lastname: "fghfgh",
				surname: "вапрп").Value,
			email: "sgf@sgeg.ef",
			description: "sgrfsdrg",
			experienceYears: 1,
			phone: Phone.Create("7897879878").Value
			);


	[Fact]
	public async Task Handle_Should_Upload_Files_To_Pet()
	{
		// arrange
		var token = new CancellationTokenSource().Token;
		Volunteer volunteer = GenerateVolunteer();
		Pet pet = GeneratePet();

		volunteer.AddPet(pet);

		var stream = new MemoryStream();
		var filename = "test.jpeg";

		var uploadFileDto = new UploadFileDto(stream, filename, "");
		List<UploadFileDto> files = [uploadFileDto, uploadFileDto];


		var command = new UploadPhotosPetCommand(volunteer.Id.Value, pet.Id.Value, files);


		fileProviderMock
			.Setup(fp => fp.UploadFilesAsync(It.IsAny<List<FileData>>(), token))
			.ReturnsAsync(new List<string> { "file1.jpg", "file2.jpg" });

		volunteerRepositoryMock
			.Setup(vr => vr.GetByIdAsync(volunteer.Id, token))
			.ReturnsAsync(volunteer);

		unitOfWorkMock
			.Setup(uow => uow.SaveChangesAsync(token))
			.Returns(Task.CompletedTask);
		unitOfWorkMock
			.Setup(tr => tr.BeginTransactionAsync(token))
			.ReturnsAsync(Mock.Of<IDbTransaction>());

		validatorMock
			.Setup(v => v.ValidateAsync(command, token))
			.ReturnsAsync(new ValidationResult());

		messageQueueMock
			.Setup(mq => mq.WriteAsync(It.IsAny<IEnumerable<FileInform>>(), token))
			.Returns(Task.CompletedTask);


		var handler = new UploadPhotosPetHandler(
			fileProviderMock.Object,
			volunteerRepositoryMock.Object,
			unitOfWorkMock.Object,
			validatorMock.Object,
			messageQueueMock.Object,
			loggerMock.Object
			);

		// act
		var result = await handler.HandleAsync(command, token);

		// assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Be(pet.Id.Value);
	}


	[Fact]
	public async Task Handle_Should_Return_Error_When_Volunteer_Does_Not_Exist()
	{
		// arrange
		var token = new CancellationTokenSource().Token;

		Volunteer volunteer = GenerateVolunteer();
		Pet pet = GeneratePet();

		volunteer.AddPet(pet);

		var stream = new MemoryStream();
		var filename = "test.jpeg";

		var uploadFileDto = new UploadFileDto(stream, filename, "");
		List<UploadFileDto> files = [uploadFileDto, uploadFileDto];


		var command = new UploadPhotosPetCommand(volunteer.Id.Value, pet.Id.Value, files);

		fileProviderMock
			.Setup(fp => fp.UploadFilesAsync(It.IsAny<List<FileData>>(), token))
			.ReturnsAsync(new List<string> { "file1.jpg", "file2.jpg" });

		volunteerRepositoryMock
			.Setup(vr => vr.GetByIdAsync(volunteer.Id, token))
			.ReturnsAsync(Result.Failure<Volunteer, Error>(Errors.General.NotFound()));

		unitOfWorkMock
			.Setup(uow => uow.SaveChangesAsync(token))
			.Returns(Task.CompletedTask);
		unitOfWorkMock
			.Setup(tr => tr.BeginTransactionAsync(token))
			.ReturnsAsync(Mock.Of<IDbTransaction>());

		validatorMock
			.Setup(v => v.ValidateAsync(command, token))
			.ReturnsAsync(new ValidationResult());

		messageQueueMock
			.Setup(mq => mq.WriteAsync(It.IsAny<IEnumerable<FileInform>>(), token))
			.Returns(Task.CompletedTask);


		var handler = new UploadPhotosPetHandler(
			fileProviderMock.Object,
			volunteerRepositoryMock.Object,
			unitOfWorkMock.Object,
			validatorMock.Object,
			messageQueueMock.Object,
			loggerMock.Object
			);

		// act
		var result = await handler.HandleAsync(command, token);

		// assert
		result.IsSuccess.Should().BeFalse();
		result.Error.Should().NotBeNull();
		result.Error.Should().BeOfType(typeof(ErrorList));
	}
}