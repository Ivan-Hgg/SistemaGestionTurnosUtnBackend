using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnosUTN.Domain.Entities;     

namespace GestionTurnosUTN.Application.Dtos
{
   
        public record IntervalCreateDTO(
       string Name,
       string Description,
       DateTime DateStart,
       DateTime DateEnd,
       Guid WorkerId,
       List<Guid> NoteIds
   );

        public record IntervalUpdateDTO(
        Guid Id,
        string Name,
        string Description,
        DateTime DateStart,
        DateTime DateEnd,
        List<Guid> NoteIds
    );
        public record IntervalDeactivateDTO(
         Guid Id,
         string? ExplainDesactivation
    );
        public record IntervalResponseDTO(
    Guid Id,
    string? Name,
    string? Description,
    DateTime DateStart,
    DateTime DateEnd,
    bool IsActive,
    string? ExplainDesactivation,
    Guid? WorkerId,
    List<NoteDTO> Notes
);
        public record NoteDTO(
    Guid Id,
    string Name
);
    
}
