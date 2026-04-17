using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTurnosUTN.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimePerTurn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TimePerTurn",
                table: "Intervals",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimePerTurn",
                table: "Intervals");
        }
    }
}
