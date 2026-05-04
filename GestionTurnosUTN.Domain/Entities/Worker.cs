using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;
public class Worker : EntityBase
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    // Navigation properties
    public ICollection<Interval>? Intervals { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<News>? News { get; set; }

    public Worker()
    {
        Name = string.Empty;
        PhoneNumber = string.Empty;
        Email = string.Empty;
        Intervals = new List<Interval>();
        Notes = new List<Note>();
        News = new List<News>();
    }
    public Worker(string name, string phoneNumber, string email)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        Intervals = new List<Interval>();
        Notes = new List<Note>();
        News = new List<News>();
    }

}
