using Dsw2025Tpi.Application.Exceptions;
using GestionTurnosUTN.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Validation;

public class TurnValidator
{
    public static void Validate(TurnModel.Request request)
    {
        if (request == null) throw new BadRequestException("Request cannot be null.");
        if (request.Date < DateTime.Now) throw new BadRequestException("Date is required.");
    }

    public static void CancelTurnValidation(TurnModel.ChangeStatusRequest request)
    {
        if(request is null) throw new BadRequestException("the request is required");
        if(string.IsNullOrEmpty(request.SecurityCode) || string.IsNullOrWhiteSpace(request.SecurityCode)) throw new BadRequestException("The securityCode is required");
        if (request.SecurityCode.Length > 8) throw new BadRequestException("the securiryCode has max lenght of 8 characters");
        if (request.SecurityCode.StartsWith(' ') || request.SecurityCode.EndsWith(' '))
            throw new BadRequestException("The securityCode can't start or end with a space");
    }

    public static void FilterTurnValidation(TurnModel.FilterTurn request)
    {
        if (request is null) throw new BadRequestException("the request is required");

    }
}
