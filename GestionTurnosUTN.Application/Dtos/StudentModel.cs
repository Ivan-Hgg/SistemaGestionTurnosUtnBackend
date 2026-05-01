using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos;

public record class StudentModel
{
    public record StudentRequest(string Name, int Legajo, string InstitutionalEmail);
}
