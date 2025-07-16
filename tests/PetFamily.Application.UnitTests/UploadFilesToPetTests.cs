using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Pets.UploadPhotos;
using PetFamily.Application.Volunteers;
using PetFamily.Contracts.DTOs;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using System.Data;
using System.Threading.Tasks;

namespace PetFamily.Application.UnitTests;

public class UploadFilesToPetTests
{
	[Fact]
	public async Task Handle_Should_Upload_Files_To_Pet()
	{
		// arrange
		var token = new CancellationTokenSource().Token;

		var volunteer = new Volunteer(
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

		var pet = new Pet(
			id: PetId.NewPeetId(),
			name: "rgeeed",
			type: PetTypes.Dog,
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
			petType: PetType.Create(BreedId.NewBreedId(), SpeciesId.NewSpeciesId()).Value
		);

		volunteer.AddPet(pet);

		var stream = new MemoryStream();
		var filename = "test.jpeg";

		var uploadFileDto = new UploadFileDto(stream, filename, "");
		List<UploadFileDto> files = [uploadFileDto, uploadFileDto];


		var command = new UploadPhotosPetCommand(volunteer.Id.Value, pet.Id.Value, files);

		var fileProviderMock = new Mock<IFileProvider>();
		fileProviderMock
			.Setup(fp => fp.UploadFilesAsync(It.IsAny<List<FileData>>(), token))
			.ReturnsAsync(new List<string> { "file1.jpg", "file2.jpg" });

		var volunteerRepositoryMock = new Mock<IVolunteerRepository>();
		volunteerRepositoryMock
			.Setup(vr => vr.GetByIdAsync(volunteer.Id, token))
			.ReturnsAsync(volunteer);

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock
			.Setup(uow => uow.SaveChangesAsync(token))
			.Returns(Task.CompletedTask);
		unitOfWorkMock
			.Setup(tr => tr.BeginTransactionAsync(token))
			.ReturnsAsync(Mock.Of<IDbTransaction>());


		var validatorMock = new Mock<IValidator<UploadPhotosPetCommand>>();
		validatorMock
			.Setup(v => v.ValidateAsync(command, token))
			.ReturnsAsync(new ValidationResult());

		var messageQueueMock = new Mock<IMessageQueue<IEnumerable<FileInform>>>();
		messageQueueMock
			.Setup(mq => mq.WriteAsync(It.IsAny<IEnumerable<FileInform>>(), token))
			.Returns(Task.CompletedTask);

		var loggerMock = new Mock<ILogger<UploadPhotosPetHandler>>();


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
}