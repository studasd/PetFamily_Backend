using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer;

public record CreateVolunteerRequest(string Firstname, string Lastname, string Surname, string Email, string Description, int ExperienceYears, string Phone);
