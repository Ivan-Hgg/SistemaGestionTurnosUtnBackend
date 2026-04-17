using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Exceptions;
using GestionTurnosUTN.Application.Dtos;
//using GestionTurnosUTN.Application.Exceptions;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Validation;
using GestionTurnosUTN.Domain.Entities;
using GestionTurnosUTN.Domain.Interfaces;

namespace GestionTurnosUTN.Application.Services;

public class TurnManagementService : ITurnManagementService
{
    private readonly IRepository _repository;
    public TurnManagementService(IRepository repository)
    {
        _repository=repository;
    }


    public async Task<TurnModel.Response> CreateTurnAsync(TurnModel.Request request)
    {
        TurnValidator.Validate(request);
        var interval = await _repository.GetById<Interval>(request.IntervalId, nameof(Interval.Notes)); if(interval == null) throw new EntityNotFoundException("Interval not found.");
        var student = await _repository.GetById<Student>(request.StudentId); if(student == null) throw new EntityNotFoundException("Student not found.");
        var note = await _repository.GetById<Note>(request.NoteId); if(note == null) throw new EntityNotFoundException("Note not found.");

        if (request.Date < interval.DateStart|| request.Date>= interval.DateEnd)
            throw new BadRequestException("El horario está fuera del intervalo.");

        if (await _repository.First<Turn>(t => t.Date== request.Date) != null) 
            throw new InvalidOperationException("The DateTime is reserved");

        if (await _repository.First<Turn>(t => t.StudentId == request.StudentId && t.NoteId == request.NoteId && t.Status == TurnStatus.PENDING && t.IntervalId == request.IntervalId) != null)
            throw new InvalidOperationException("the student already have a Turn on the interval with the same note");

        if(!interval.Notes.Any(n => n.Id == request.NoteId)) throw new BadRequestException("Note isn't already associated with the interval.");
        
        //para reslver lo del timePerTurn
        var diff = (request.Date - interval.DateStart).TotalMinutes;
        if (diff < 0 || diff % interval.TimePerTurn != 0)
            throw new BadRequestException("The schedule does not match the available shifts.");

        string securityCode;  
        do
        {
            securityCode= Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        } while (await _repository.First<Turn>(t => t.SecurityCode == securityCode) != null);
        
        var turn = new Turn
        {
            Date = request.Date,
            SecurityCode= securityCode,
            IntervalId = request.IntervalId,
            StudentId = request.StudentId,
            NoteId = request.NoteId
        };
        await _repository.Add<Turn>(turn); 
        return new TurnModel.Response(
            Guid.NewGuid(),
            turn.SecurityCode,
            turn.Date,
            TurnStatus.PENDING.ToString(),
            turn.IntervalId,
            turn.StudentId,
            turn.NoteId
        );
    }

    public async Task CancelTurnAsync(TurnModel.CancelRequest request)
    {
        var turn = await _repository.GetById<Turn>(request.Id); if(turn ==null)throw new EntityNotFoundException("Turn doesn't found");
        turn.Status = TurnStatus.CANCELLED;
        await _repository.Update(turn);
        return;

    }


}
