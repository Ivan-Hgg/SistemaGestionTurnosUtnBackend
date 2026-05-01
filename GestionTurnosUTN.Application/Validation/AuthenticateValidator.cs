using Dsw2025Tpi.Application.Exceptions;
using GestionTurnosUTN.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Validation;

public static class AuthenticateValidator
{
    public static void ValidateRegisterModelRequest(RegisterModel model)
    {
        if (model == null)
            throw new BadRequestException("El modelo de registro no puede ser nulo.");
        ValidateUsername(model.Username);
        ValidateRole(model.Role);
        if (model.Role.ToUpper() == "WORKER" && model.Worker!=null)
        {
            WorkerValidator.WorkerModelRequestValidator(model.Worker);
        } else if(model.Role.ToUpper() =="STUDENT" && model.Student!=null)
        {
            StudentValidator.StudentModelRequestValidate(model.Student);
        }
        ValidatePassword(model.Password);
    }

    public static void ValidateLoginModelRequest(LoginModelRequest model) 
    {
        if (model == null) throw new BadRequestException("El modelo de logeo no puede ser nulo");
        ValidateUsername(model.Username);
        ValidatePassword(model.Password);
    }

    public static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BadRequestException("El nombre de usuario es obligatorio.");
        if (username.Length < 3 || username.Length > 50)
            throw new BadRequestException("El nombre de usuario debe tener entre 3 y 50 caracteres.");
        if (username.StartsWith(' ') || username.EndsWith(' '))
            throw new BadRequestException("El nombre de usuario no puede comenzar o terminar con un espacio en blanco.");
        var pattern = @"^[a-zA-Z0-9._-]+$";
        if(!Regex.IsMatch(username, pattern))
            throw new BadRequestException($"El nombre de usuario es invalido: {username}" +
                "\nsolo puede contener letras, números, puntos, guiones bajos y guiones.");
    }

    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("La contraseña es obligatoria.");
    }

    public static void ValidateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new BadRequestException("El rol es obligatorio.");
        if (role.ToUpper() != "WORKER" && role.ToUpper() != "STUDENT" && role.ToUpper() != "SUPREME")
            throw new BadRequestException($"El rol ingresado es desconocido: {role} "+ " Posibles roles: 'WORKER' o 'STUDENT' o 'SUPREME'.");
    }


}
