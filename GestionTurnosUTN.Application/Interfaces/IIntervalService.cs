using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnosUTN.Application.Dtos;

namespace GestionTurnosUTN.Application.Interfaces
{
    
        public interface IIntervalService
        {
            Task<IntervalResponseDTO> CreateAsync(IntervalCreateDTO dto);
            Task<IntervalResponseDTO> UpdateAsync(IntervalUpdateDTO dto);
            Task DeactivateAsync(IntervalDeactivateDTO dto);

            Task<IntervalResponseDTO?> GetByIdAsync(Guid id);
            Task<List<IntervalResponseDTO>> GetAllAsync();
        }
    
}
