using PetFamily.Application.Authorization.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Authorization;

public interface ITokenProvider
{
	string GenerateAccessToken(User user);
}
