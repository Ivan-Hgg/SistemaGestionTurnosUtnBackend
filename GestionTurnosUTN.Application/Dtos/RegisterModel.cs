using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos;

public record RegisterModel(
    string Username,
    StudentModel.StudentRequest? Student,
    WorkerModel.WorkerRequest? Worker,
    string Password,
    string Role
);

public record RegisterModelResponse(
    Guid? Id,
    string Username,
    string Role
);
