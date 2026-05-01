using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos;

public record class WorkerModel
{
    public record WorkerRequest(string Name, string PhoneNumber, string Email);
}
