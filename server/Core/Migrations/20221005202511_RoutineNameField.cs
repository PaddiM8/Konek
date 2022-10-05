using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konek.Server.Core.Migrations
{
    public partial class RoutineNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoutineDefinitions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoutineDefinitions");
        }
    }
}
