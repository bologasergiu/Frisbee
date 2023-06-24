using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrisbeeApp.Context.Migrations
{
    public partial class PlayerAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrainingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrainingId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrainingId1",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainingId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingId1",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrainingId",
                table: "AspNetUsers",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrainingId1",
                table: "AspNetUsers",
                column: "TrainingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId",
                table: "AspNetUsers",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId1",
                table: "AspNetUsers",
                column: "TrainingId1",
                principalTable: "Trainings",
                principalColumn: "Id");
        }
    }
}
