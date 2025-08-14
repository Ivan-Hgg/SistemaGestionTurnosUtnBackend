using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;
public class Note: EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // Navigation properties
    public Guid WorkerId { get; set; }
    public Worker? Worker { get; set; }
    public IEnumerable<Turn>? Turns { get; set; }
    public IEnumerable<Interval>? Intervals { get; set; }

    public Note()
    {
        Name = string.Empty;
    }
    public Note(string name, Guid workerId)
    {
        Name = name;
        WorkerId = workerId;
    }
}
