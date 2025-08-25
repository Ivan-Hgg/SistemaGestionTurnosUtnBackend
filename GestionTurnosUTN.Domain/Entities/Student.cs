using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;
public class Student : EntityBase
{
    public string Name { get; set; }
    public string InstitutionalEmail { get; set; }
    public int Legajo { get; set; }
    // Navigation properties
    public IEnumerable<Turn>? Turns { get; set; }
    public Student(string name, string institutionalEmail, int legajo)
    {
        Name = name;
        InstitutionalEmail = institutionalEmail;
        Legajo = legajo;
        Turns = new List<Turn>();
    }
}
