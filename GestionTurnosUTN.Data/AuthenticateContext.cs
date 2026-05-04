using GestionTurnosUTN.Data.Identity;
using GestionTurnosUTN.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Data;

public class AuthenticateContext : IdentityDbContext<IdentityUserExtension>
{
    public AuthenticateContext(DbContextOptions<AuthenticateContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<Student>();
        builder.Ignore<Worker>();
        builder.Ignore<Turn>();
        builder.Ignore<Note>();
        builder.Ignore<News>();
        builder.Ignore<Interval>();
        builder.Ignore<EntityBase>();
        builder.Entity<Student>(e =>
        {
            e.HasKey(c => c.Id);
            e.ToTable("Students", tb => tb.ExcludeFromMigrations());
        });
        builder.Entity<Worker>(e =>
        {
            e.HasKey(c => c.Id);
            e.ToTable("Workers", tb => tb.ExcludeFromMigrations());
        });

        builder.Entity<IdentityUserExtension>(b =>
        {
            b.ToTable("Usuarios");
            b.HasOne<Student>()
                .WithOne()
                .HasForeignKey<IdentityUserExtension>(c => c.StudenId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne<Worker>()
                .WithOne()
                .HasForeignKey<IdentityUserExtension>(c => c.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<IdentityRole>(b => { b.ToTable("Roles"); });
        builder.Entity<IdentityUserRole<string>>(b => { b.ToTable("UsuariosRoles"); });
        builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UsuariosClaims"); });
        builder.Entity<IdentityUserLogin<string>>(b => { b.ToTable("UsuariosLogins"); });
        builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RolesClaims"); });
        builder.Entity<IdentityUserToken<string>>(b => { b.ToTable("UsuariosTokens"); });
    }

}
