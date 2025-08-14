using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Domain.Entities;

public class News: EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description{ get; set; }
    public DateTime DatePost { get; set; }
    public Boolean IsActive { get; set; }
    public NewsStatus Status { get; set; }
    public Guid? WorkerId { get; set; }
    public Worker? Worker { get; set; }
    
    public News(string title, string description, DateTime datePost, Guid? workerId)
    {
        Title = title;
        Description = description;
        DatePost = datePost;
        IsActive = true;
        Status = NewsStatus.PENDING;
        WorkerId = workerId;
    }
}
