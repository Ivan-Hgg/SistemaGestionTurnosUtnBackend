using GestionTurnosUTN.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Data;

public class GestionTurnosUTNDomainContext : DbContext
{
    public DbSet<Interval> Intervals { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Turn> Turns { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<News> News { get; set; }
    public GestionTurnosUTNDomainContext(DbContextOptions<GestionTurnosUTNDomainContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Interval>(eb=> 
            {
                eb.ToTable("Intervals");
                eb.HasKey(i => i.Id);
                eb.Property(i => i.Name).IsRequired().HasMaxLength(100);
                eb.Property(i => i.Description).HasMaxLength(250);
                eb.Property(i => i.DateStart).IsRequired();
                eb.Property(i => i.DateEnd).IsRequired();
                eb.Property(i => i.IsActive).IsRequired();
                eb.Property(i => i.ExplainDesactivation).HasMaxLength(250);
                // Relationships
                eb.HasOne(i => i.Worker)
                  .WithMany(s => s.Intervals)
                  .HasForeignKey(i => i.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
                eb.HasMany(i => i.Turns)
                  .WithOne(t => t.Interval)
                  .HasForeignKey(t => t.IntervalId)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasMany(i => i.Notes)
                    .WithMany(n => n.Intervals);
            }
        );
        modelBuilder.Entity<Note>(eb => 
            {
                eb.ToTable("Notes");
                eb.HasKey(n => n.Id);
                eb.Property(n => n.Name).IsRequired().HasMaxLength(100);
                // Relationships
                eb.HasOne(n => n.Worker)
                  .WithMany(w => w.Notes)
                  .HasForeignKey(n => n.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
                eb.HasMany(n => n.Turns)
                  .WithOne(t => t.Note)
                  .HasForeignKey(t => t.NoteId)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasMany(n => n.Intervals)
                    .WithMany(i => i.Notes);
            }
        );
        modelBuilder.Entity<Student>(eb => 
            {
                eb.ToTable("Students");
                eb.HasKey(s => s.Id);
                eb.Property(s => s.Name).IsRequired().HasMaxLength(100);
                eb.Property(s => s.InstitutionalEmail).IsRequired().HasMaxLength(150);
                eb.Property(s => s.Legajo).IsRequired();
                // Relationships
                eb.HasMany(s => s.Turns)
                  .WithOne(t => t.Student)
                  .HasForeignKey(t => t.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
            }
        );
        modelBuilder.Entity<Turn>(eb => 
            {
                eb.ToTable("Turns");
                eb.HasKey(t => t.Id);
                eb.Property(t => t.SecurityCode).IsRequired().HasMaxLength(50);
                eb.Property(t => t.Date).IsRequired();
                eb.Property(t => t.DateAttended);
                eb.Property(t => t.Status).IsRequired();
                // Relationships
                eb.HasOne(t => t.Interval)
                  .WithMany(i => i.Turns)
                  .HasForeignKey(t => t.IntervalId)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(t => t.Student)
                  .WithMany(s => s.Turns)
                  .HasForeignKey(t => t.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(t => t.Note)
                  .WithMany(n => n.Turns)
                  .HasForeignKey(t => t.NoteId)
                  .OnDelete(DeleteBehavior.Cascade);
            }
        );
        modelBuilder.Entity<Worker>(eb => 
            {
                eb.ToTable("Workers");
                eb.HasKey(w => w.Id);
                eb.Property(w => w.Name).IsRequired().HasMaxLength(100);
                eb.Property(w => w.Email).IsRequired().HasMaxLength(150);
                eb.Property(w => w.PhoneNumber).IsRequired().HasMaxLength(20);
                // Relationships
                eb.HasMany(w => w.Intervals)
                  .WithOne(i => i.Worker)
                  .HasForeignKey(i => i.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
                eb.HasMany(w => w.Notes)
                  .WithOne(n => n.Worker)
                  .HasForeignKey(n => n.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
                eb.HasMany(w => w.News)
                    .WithOne(n => n.Worker)
                    .HasForeignKey(n => n.WorkerId)
                    .OnDelete(DeleteBehavior.SetNull);
            }
        );
        modelBuilder.Entity<News>(eb => 
            {
                eb.ToTable("News");
                eb.HasKey(n => n.Id);
                eb.Property(n => n.Title).IsRequired().HasMaxLength(80);
                eb.Property(n => n.Description).IsRequired().HasMaxLength(500);
                eb.Property(n => n.DatePost).IsRequired();
                eb.Property(n => n.IsActive).IsRequired();
                eb.Property(n => n.Status).IsRequired();
                // Relationships
                eb.HasOne(n => n.Worker)
                  .WithMany(w => w.News)
                  .HasForeignKey(n => n.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
            }
        );

    }
}
