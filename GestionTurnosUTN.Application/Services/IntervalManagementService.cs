using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnosUTN.Domain.Entities;
using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnosUTN.Application.Services
{
    public class IntervalManagementService : IIntervalService
    {
        private readonly IRepository _repository;
        //private readonly IUnitOfWork _unitOfWork;

        public IntervalManagementService(IRepository repository /*,IUnitOfWork unitOfWork*/)
        {
            _repository = repository;
            /*_unitOfWork = unitOfWork;*/
        }

        public async Task<IntervalResponseDTO> CreateAsync(IntervalCreateDTO dto)
        {
            /* 1️⃣ Validaciones
            if (dto.DateEnd <= dto.DateStart)
                throw new Exception("La fecha fin debe ser mayor");

            if (dto.NoteIds == null || !dto.NoteIds.Any())
                throw new Exception("Debe tener al menos una nota");*/

            // 2️⃣ Obtener Notes 
            var notes = await _repository.GetFiltered<Note>(n => dto.NoteIds.Contains(n.Id));

            if (notes == null || notes.Count() != dto.NoteIds.Count)
                throw new Exception("Notas inválidas");

            // 3️⃣ Crear entidad
            var interval = new Interval(
                dto.Name,
                dto.Description,
                dto.DateStart,
                dto.DateEnd,
                dto.WorkerId,
                notes.ToList()
            );

            // 4️⃣ Guardar
            await _repository.Add(interval);


     
            //await _context.SaveChangesAsync(); // o UnitOfWork

            // 5️⃣ Mapear respuesta
            return new IntervalResponseDTO(
                interval.Id,
                interval.Name,
                interval.Description,
                interval.DateStart,
                interval.DateEnd,
                interval.IsActive,
                interval.ExplainDesactivation,
                interval.WorkerId,
                interval.Notes.Select(n => new NoteDTO(n.Id, n.Name)).ToList()
            );


        }
        public async Task<IntervalResponseDTO> UpdateAsync(IntervalUpdateDTO dto)
        {
            try
            {
                var intervalo = await _repository.GetById<Interval>(dto.Id, "Notas");

                if (intervalo == null)
                    throw new Exception("Intervalo no encontrado");

                var notes = await _repository.GetFiltered<Note>(n => dto.NoteIds.Contains(n.Id));

                if (notes == null || notes.Count() != dto.NoteIds.Count)
                    throw new Exception("Notas inválidas");

                intervalo.Name = dto.Name;
                intervalo.Description = dto.Description;
                intervalo.DateStart = dto.DateStart;
                intervalo.DateEnd = dto.DateEnd;
                intervalo.Notes = notes.ToList();

                await _repository.Update(intervalo);

                // 🔹 Mapear a DTO de respuesta
                return new IntervalResponseDTO(
                 intervalo.Id,
                 intervalo.Name,
                 intervalo.Description,
                 intervalo.DateStart,
                 intervalo.DateEnd,
                 intervalo.IsActive,
                 intervalo.ExplainDesactivation,
                 intervalo.WorkerId,
                 intervalo.Notes.Select(n => new NoteDTO(n.Id, n.Name)).ToList()

                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar intervalo: {ex.Message}");
            }
        }

        public async Task DeactivateAsync(IntervalDeactivateDTO dto)
        {
            try
            {
                // 🔹 Buscar el intervalo
                var intervalo = await _repository.GetById<Interval>(dto.Id);

                if (intervalo == null)
                    throw new Exception("Intervalo no encontrado");

                // 🔹 Validar si ya está desactivado
                if (!intervalo.IsActive)
                    throw new Exception("El intervalo ya está desactivado");

                // 🔹 Desactivar
                intervalo.IsActive = false;
                intervalo.ExplainDesactivation = dto.ExplainDesactivation;

                // 🔹 Guardar cambios
                await _repository.Update(intervalo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IntervalResponseDTO?> GetByIdAsync(Guid id)
        {
            var intervalo = await _repository.GetById<Interval>(id, "Notes");

            if (intervalo == null)
                return null;

            return new IntervalResponseDTO(
                intervalo.Id,
                intervalo.Name,
                intervalo.Description,
                intervalo.DateStart,
                intervalo.DateEnd,
                intervalo.IsActive,
                intervalo.ExplainDesactivation,
                intervalo.WorkerId,
                intervalo.Notes.Select(n => new NoteDTO(n.Id, n.Name)).ToList()
            );
        }

        public async Task<List<IntervalResponseDTO>> GetAllAsync()
        {
            var intervalos = await _repository.GetAll<Interval>("Notes");

            return intervalos.Select(intervalo => new IntervalResponseDTO(
                intervalo.Id,
                intervalo.Name,
                intervalo.Description,
                intervalo.DateStart,
                intervalo.DateEnd,
                intervalo.IsActive,
                intervalo.ExplainDesactivation,
                intervalo.WorkerId,
                intervalo.Notes.Select(n => new NoteDTO(n.Id, n.Name)).ToList()
            )).ToList();
        }



    }
}
