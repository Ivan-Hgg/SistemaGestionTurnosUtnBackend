using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities
{
    public enum TurnStatus
    {
        PENDING,
        ATTENDED,
        LOST,
        CANCELLED
    }
}