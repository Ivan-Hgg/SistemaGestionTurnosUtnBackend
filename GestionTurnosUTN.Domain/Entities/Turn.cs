using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;
public class Turn: EntityBase
{
    public string SecurityCode { get; set; }
    public DateTime Date { get; set; }
    public DateTime? DateAttended { get; set; }
    public TurnStatus Status { get; set; }
    // Navigation properties
    public Guid IntervalId { get; set; }
    public Interval? Interval { get; set; }
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
    public Guid NoteId { get; set; }
    public Note? Note { get; set; }

    public Turn()
    {
        SecurityCode = string.Empty;
        Status = TurnStatus.PENDING;
    }
}
