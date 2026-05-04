using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos;

public record RegisterStudentModel(
    string Username,
    StudentModel.StudentRequest Student,
    string Password
);

public record RegisterWorkerModel(
    string Username,
    WorkerModel.WorkerRequest Worker,
    string Password
);

public record RegisterModelResponse(
    Guid? Id,
    string Username
);
