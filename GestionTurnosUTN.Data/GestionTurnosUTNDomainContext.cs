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
                /*eb.HasOne(i => i.Worker)
                  .WithMany(w => w.Intervals)
                  .HasForeignKey(i => i.WorkerId)
                  .OnDelete(DeleteBehavior.SetNull);
                */
            }


            );


    }
}
