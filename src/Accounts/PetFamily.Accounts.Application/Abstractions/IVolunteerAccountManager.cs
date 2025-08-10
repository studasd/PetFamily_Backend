using PetFamily.Accounts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Abstractions;

public interface IVolunteerAccountManager
{
	Task CreateVolunteerAccount(VolunteerAccount volunteerAccount, CancellationToken token);
}
