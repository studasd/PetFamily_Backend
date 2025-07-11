﻿using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
	Task<Result<string, Error>> UploadFileAsync(FileData fileData, CancellationToken token = default);
}
