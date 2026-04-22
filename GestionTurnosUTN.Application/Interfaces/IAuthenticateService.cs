using GestionTurnosUTN.Application.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Interfaces;

public interface IAuthenticateService
{
    Task<RegisterModelResponse> RegisterAsync(RegisterModel model);
    Task<LoginModelResponse> LoginAsync(LoginModelRequest model);
}
