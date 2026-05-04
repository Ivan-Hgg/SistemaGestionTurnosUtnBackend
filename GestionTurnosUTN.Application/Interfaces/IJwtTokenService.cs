using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string username, string role);
    string GenerateTokenMoreRoles(string username, List<string> roles);
}
