using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Interfaces;

public interface ITurnManagementService
{
     Task<TurnModel.Response> CreateTurnAsync(TurnModel.Request request);
     Task CancelTurnAsync(TurnModel.ChangeStatusRequest request);
    Task<TurnModel.ResponsePagination> GetTurnsAsync(TurnModel.FilterTurn request);
    Task AttendedTurn(TurnModel.ChangeStatusRequest request);
    Task LostTurn(TurnModel.ChangeStatusRequest request);
}
