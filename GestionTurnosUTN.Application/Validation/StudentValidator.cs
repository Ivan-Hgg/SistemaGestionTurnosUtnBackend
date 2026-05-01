using Dsw2025Tpi.Application.Exceptions;
using GestionTurnosUTN.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Validation;

public class StudentValidator
{
    public static void StudentModelRequestValidate(StudentModel.StudentRequest request)
    {

        if (request == null)
            throw new BadRequestException("The Worker can't be Null.");
        ValidateName(request.Name);
        ValidateEmail(request.InstitutionalEmail);
    }
    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new BadRequestException("El email es obligatorio.");
        if (email.Length < 5 || email.Length > 254)
            throw new BadRequestException("El email debe tener entre 5 y 254 caracteres.");
        if (email.StartsWith('.') || email.EndsWith('.'))
            throw new BadRequestException("El email no puede comenzar o terminar con un punto '.'");
        if (!email.EndsWith("alu.frt.utn.edu.ar"))
            throw new BadRequestException("The mail only can be an Institutional mail");
        var pattern = @"^[a-zA-Z0-9.!#$%&'+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)+$";
        var regex = new Regex(pattern);
        if (!regex.IsMatch(email))
            throw new BadRequestException($"Direccion de Email invalida: {email}");
    }

    public static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("El nombre es obligatorio.");
        if (name.Length < 3 || name.Length > 50)
            throw new BadRequestException("El nombre debe tener entre 3 y 50 caracteres.");
        if (name.StartsWith(' ') || name.EndsWith(' '))
            throw new BadRequestException("El nombre no puede comenzar o terminar con un espacio en blanco.");
    }
    
}
