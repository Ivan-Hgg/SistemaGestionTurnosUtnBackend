using GestionTurnosUTN.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Data.Identity;

public class IdentityUserExtension : IdentityUser
{
    public Guid? WorkerId { get; init; } // Fk
    public Guid? StudenId { get; init; } // Fk


}
