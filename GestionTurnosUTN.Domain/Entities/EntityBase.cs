using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;

public class EntityBase
{
    Guid ID { get; set; }
    public EntityBase()
    {
        ID = Guid.NewGuid();
    }
}
