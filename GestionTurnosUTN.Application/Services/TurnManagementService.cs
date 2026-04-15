using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Domain.Entities;
using GestionTurnosUTN.Domain.Interfaces;

namespace GestionTurnosUTN.Application.Services;

public class TurnManagementService : ITurnManagementService
{
    private readonly IRepository _repository;


    public async Task<TurnModel.Response> CreateTurnAsync(TurnModel.Request request)
    {
       
    }

}
