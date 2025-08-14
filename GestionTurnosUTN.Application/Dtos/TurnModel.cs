using GestionTurnosUTN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos;

public class TurnModelRequest(
    string SecurityCode,
    DateTime Date,
    TurnStatus Status,
    Guid IntervalId,
    Guid StudentId,
    Guid NoteId
);

