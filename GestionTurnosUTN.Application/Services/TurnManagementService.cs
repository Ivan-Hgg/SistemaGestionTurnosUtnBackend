using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnosUTN.Application.Exceptions;
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

        if (await _repository.First<Turn>(t => t.Date== request.Date && t.Status==TurnStatus.PENDING) != null) 
            throw new InvalidOperationException("The DateTime is reserved");

        if (await _repository.First<Turn>(t => t.StudentId == request.StudentId && t.NoteId == request.NoteId && t.Status == TurnStatus.PENDING && t.IntervalId == request.IntervalId) != null)
            throw new InvalidOperationException("the student already have a Turn on the interval with the same note");

        if(!interval.Notes.Any(n => n.Id == request.NoteId)) throw new BadRequestException("Note isn't already associated with the interval.");
        
        //para resolver lo del timePerTurn
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

    public async Task CancelTurnAsync(TurnModel.ChangeStatusRequest request)
    {
        TurnValidator.CancelTurnValidation(request);
        var turn = await _repository.GetById<Turn>(request.Id); if(turn ==null)throw new EntityNotFoundException("Turn doesn't found");
        //conviene hacer una validacion por si el turno ya estaba cancelado?
        if (turn.Status != TurnStatus.PENDING)
            throw new InvalidOperationException("Cant Cancel a Turn Attended or Lost");
        TimeSpan duration= DateTime.Now-turn.Date;
        if (duration.TotalDays > 3) throw new InvalidOperationException("You can't cancel a turn after 3 days of take it");
        if (turn.SecurityCode != request.SecurityCode) throw new BadRequestException("The SecurityCode is incorrect");

        turn.Status = TurnStatus.CANCELLED;
        await _repository.Update(turn);
        return;
    }

    public async Task<TurnModel.ResponsePagination> GetTurnsAsync(TurnModel.FilterTurn request)
    {
        TurnValidator.FilterTurnValidation(request);
        var status= request.Status =="PENDING" ? TurnStatus.PENDING : 
            request.Status== "ATTENDED"? TurnStatus.ATTENDED:
            request.Status== "LOST" ? TurnStatus.LOST:
            request.Status== "CANCELLED" ? TurnStatus.CANCELLED:
            (TurnStatus?)null;

        var filteredTurns = await _repository.GetFiltered<Turn>(t=> (
            (status==null || t.Status==status) 
            && (request.Date!=null? t.Date == request.Date 
                : (request.DateStart == null || t.Date >= request.DateStart)
                &&(request.DateEnd == null || t.Date <= request.DateEnd))   
            && (request.IntervalId==null || t.IntervalId== request.IntervalId) 
            && (request.NoteId == null || t.NoteId==request.NoteId))
            && (request.Search==null || (t.Student != null && t.Student.Legajo == request.Search))
            , nameof(Turn.Student));
        if (filteredTurns is null || !filteredTurns.Any()) throw new NoContentException("No turns were found");
        var turns = filteredTurns.Select(t=> new TurnModel.Response(
            t.Id,
            t.SecurityCode,
            t.Date,
            t.Status.ToString(),
            t.IntervalId,
            t.StudentId,
            t.NoteId
            ))
        .OrderBy(t=>t.Date)
        .Skip((request.PageNumber-1) * request.PageSize??0)
        .Take(request.PageSize ?? filteredTurns.Count());
        
        return new TurnModel.ResponsePagination(turns.ToList(), filteredTurns.Count());

    }
    //me conviene hacer un solo metodo de cambio de estado o hacer por separado para darle mayor seguridad?
    public async Task ChangeStatusTurn(TurnModel.StatusRequest request)
    {
        if (request == null) throw new BadRequestException("The request is required");
        var status = request.Status == "PENDING" ? TurnStatus.PENDING :
            request.Status == "ATTENDED" ? TurnStatus.ATTENDED :
            request.Status == "LOST" ? TurnStatus.LOST :
            request.Status == "CANCELLED" ? TurnStatus.CANCELLED :
            (TurnStatus?)null;
        var turn = await _repository.GetById<Turn>(request.Id) ?? throw new EntityNotFoundException("there is no Turn with that Id.");
        if (status == TurnStatus.CANCELLED)
            if (turn.SecurityCode != request.SecurityCode) throw new BadRequestException("The SecurityCode is incorrect");
    }

    public async Task AttendedTurn (TurnModel.ChangeStatusRequest request)
    {
        if (request == null) throw new BadRequestException("The request is required");
        var turn = await _repository.GetById<Turn>(request.Id) ?? throw new EntityNotFoundException("there is no Turn with that Id.");
        if (turn.DateAttended != null) throw new InvalidOperationException("The turn has already attended");
        turn.Status = TurnStatus.ATTENDED;
        turn.DateAttended=DateTime.Now;
        await _repository.Update(turn);
        return;
    }
    public async Task LostTurn(TurnModel.ChangeStatusRequest request)
    {
        if (request == null) throw new BadRequestException("The request is required");
        var turn = await _repository.GetById<Turn>(request.Id) ?? throw new EntityNotFoundException("there is no Turn with that Id.");
        if (turn.DateAttended != null) throw new InvalidOperationException("The turn has been attended");
        turn.Status = TurnStatus.LOST;
        await _repository.Update(turn);
        return;
    }
}
