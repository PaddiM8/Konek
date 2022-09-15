using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konek.Server.Core.Migrations
{
    public partial class InitialModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Lamps",
                columns: table => new
                {
                    LampId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RemoteId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lamps", x => x.LampId);
                });

            migrationBuilder.CreateTable(
                name: "RoutineDefinitions",
                columns: table => new
                {
                    RoutineDefinitionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineDefinitions", x => x.RoutineDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "GroupLamp",
                columns: table => new
                {
                    GroupsGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    LampsLampId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLamp", x => new { x.GroupsGroupId, x.LampsLampId });
                    table.ForeignKey(
                        name: "FK_GroupLamp_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLamp_Lamps_LampsLampId",
                        column: x => x.LampsLampId,
                        principalTable: "Lamps",
                        principalColumn: "LampId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Effects",
                columns: table => new
                {
                    EffectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Brightness = table.Column<byte>(type: "INTEGER", nullable: false),
                    Temperature = table.Column<byte>(type: "INTEGER", nullable: false),
                    RoutineDefinitionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.EffectId);
                    table.ForeignKey(
                        name: "FK_Effects_RoutineDefinitions_RoutineDefinitionId",
                        column: x => x.RoutineDefinitionId,
                        principalTable: "RoutineDefinitions",
                        principalColumn: "RoutineDefinitionId");
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    RoutineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DefinitionRoutineDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Expiry = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    LampId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.RoutineId);
                    table.ForeignKey(
                        name: "FK_Routines_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_Routines_Lamps_LampId",
                        column: x => x.LampId,
                        principalTable: "Lamps",
                        principalColumn: "LampId");
                    table.ForeignKey(
                        name: "FK_Routines_RoutineDefinitions_DefinitionRoutineDefinitionId",
                        column: x => x.DefinitionRoutineDefinitionId,
                        principalTable: "RoutineDefinitions",
                        principalColumn: "RoutineDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Effects_RoutineDefinitionId",
                table: "Effects",
                column: "RoutineDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupLamp_LampsLampId",
                table: "GroupLamp",
                column: "LampsLampId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_DefinitionRoutineDefinitionId",
                table: "Routines",
                column: "DefinitionRoutineDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_GroupId",
                table: "Routines",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_LampId",
                table: "Routines",
                column: "LampId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Effects");

            migrationBuilder.DropTable(
                name: "GroupLamp");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Lamps");

            migrationBuilder.DropTable(
                name: "RoutineDefinitions");
        }
    }
}
