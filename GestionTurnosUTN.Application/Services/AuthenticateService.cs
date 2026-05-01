
using Dsw2025Tpi.Application.Exceptions;
using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Validation;
using GestionTurnosUTN.Data.Identity;
using GestionTurnosUTN.Domain.Entities;
using GestionTurnosUTN.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IRepository _repository;
    private readonly UserManager<IdentityUserExtension> _userManager;
    private readonly SignInManager<IdentityUserExtension> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthenticateService> _logger;

    public AuthenticateService(IRepository repository, UserManager<IdentityUserExtension> userManager
        , SignInManager<IdentityUserExtension> signInManager, IJwtTokenService jwtTokenService, ILogger<AuthenticateService> logger)
    {
        _repository = repository;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }
    public async Task<LoginModelResponse> LoginAsync(LoginModelRequest model)
    {
        AuthenticateValidator.ValidateLoginModelRequest(model);

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is null)
            throw new InvalidOperationException("El usuario no tiene roles asignados.");
        var token = _jwtTokenService.GenerateToken(model.Username, role);
        _logger.LogInformation($"Se logueó el usuario: {model.Username}");
        return new LoginModelResponse(token, role); 
    }
    //BORRAR ESTO
    //public async Task<RegisterModelResponse> RegisterAsync(RegisterModel model)
    //{
    //    return new RegisterModelResponse(
    //            null,
    //            string.Empty,
    //            model.Role.ToUpper()
    //        );
    //}

    public async Task<RegisterModelResponse> RegisterAsync(RegisterModel model)
    {
        AuthenticateValidator.ValidateRegisterModelRequest(model);

        var existUser = await _userManager.FindByNameAsync(model.Username);
        if (existUser != null) throw new DuplicatedEntityException($"El nombre de usuario {model.Username} ya existe.");
        if (model.Role.ToUpper() == "STUDENT" && model.Student != null)
        {
            var existmail = await _repository.First<Student>(c => c.InstitutionalEmail == model.Student.InstitutionalEmail);
            if (existmail != null) throw new DuplicatedEntityException($"Un estudiante ya fue registrado con el EMAIL: {model.Student.InstitutionalEmail}");

            var existLegajo = await _repository.First<Student>(c => c.Legajo == model.Student.Legajo);
            if (existLegajo != null) throw new DuplicatedEntityException($"Un cliente ya fue registrado el nuemro de legajo: {model.Student.Legajo}");

            var student = new Student(model.Student.Name, model.Student.InstitutionalEmail, model.Student.Legajo);
            var user = new IdentityUserExtension { StudenId = student.Id, UserName = model.Username, Email = model.Student.InstitutionalEmail };

            var resultStudent = await _repository.Add(student);
            if (resultStudent is null)
            {
                throw new DataInsertException("Error al crear el cliente");
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                await _repository.Delete(student);
                throw new DataInsertException(result.Errors.ToString());
            }
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role.ToUpper());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                await _repository.Delete(student);
                throw new DataInsertException("Error Asignando Rol al usuario");
            }

            _logger.LogInformation($"Se registró un nuevo usuario: {model.Username} con rol {model.Role.ToUpper()}");
            return new RegisterModelResponse(
                student.Id,
                user.UserName,
                model.Role.ToUpper()
            );
        }
        //falta corregir este else y probar todos los posibles flujos en el registro
        else if (model.Role.ToUpper() == "WORKER" && model.Worker != null)
        {
            var existmail = await _repository.First<Worker>(c => c.Email == model.Worker.Email);
            if (existmail != null) throw new DuplicatedEntityException($"Un worker ya fue registrado con el EMAIL: {model.Worker.Email}");

            var existPhoneNumber = await _repository.First<Worker>(c => c.PhoneNumber == model.Worker.PhoneNumber);
            if (existPhoneNumber != null) throw new DuplicatedEntityException($"Un worker ya fue registrado el nuemro de telefono: {model.Worker.PhoneNumber}");

            var worker = new Worker(model.Worker.Name, model.Worker.PhoneNumber, model.Worker.Email);

            var user = new IdentityUserExtension { WorkerId = worker.Id, UserName = model.Username, Email = model.Worker.Email };

            var resultWorker = await _repository.Add(worker);
            if (resultWorker is null)
            {
                throw new DataInsertException("Error al crear el Worker");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                await _repository.Delete(worker);
                throw new DataInsertException(result.Errors.ToString());
            }
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role.ToUpper());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                await _repository.Delete(worker);

                throw new DataInsertException("Error Asignando Rol al usuario");
            }

            _logger.LogInformation($"Se registró un nuevo usuario: {model.Username} con rol {model.Role.ToUpper()}");
            return new RegisterModelResponse(
                worker.Id,
                user.UserName,
                model.Role.ToUpper()
            );
        }
        else throw new Exception("the Role Doesn't match its respective entity");




    }
}
