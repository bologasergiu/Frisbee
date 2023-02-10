using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrisbeeApp.Context.Migrations
{
    public partial class AddTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Teams_TeamId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TeamId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Trainings");

            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "Trainings");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Trainings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TeamId",
                table: "Trainings",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Teams_TeamId",
                table: "Trainings",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
