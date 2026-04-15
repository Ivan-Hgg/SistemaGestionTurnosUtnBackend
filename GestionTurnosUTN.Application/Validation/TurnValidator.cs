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
}
