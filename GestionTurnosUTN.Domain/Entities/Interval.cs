using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;

public class Interval:EntityBase
{
    public string? Name { get; set; }
    public string? Description{ get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    //public int TurnQuantity{ get; set; }
    public Boolean IsActive { get; set; }
    public string? ExplainDesactivation { get; set; }
    //Relationships
    public Guid? WorkerId { get; set; }
    public Worker? Worker { get; set; }
    public IEnumerable<Turn>? Turns { get; set; }
    public IEnumerable<Note> Notes { get; set; }


    public Interval()
    {
        IsActive = true;
        ExplainDesactivation = string.Empty;
        Notes = [];
    }
    public Interval(string name, string description, DateTime dateStart, DateTime dateEnd, Guid workerId, IEnumerable<Note> notes)
    {
        Name = name;
        Description = description;
        DateStart = dateStart;
        DateEnd = dateEnd;
        IsActive = true;
        ExplainDesactivation = string.Empty;
        WorkerId = workerId;
        Notes= notes;
    }
}
