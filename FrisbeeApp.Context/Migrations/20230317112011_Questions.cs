using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrisbeeApp.Context.Migrations
{
    public partial class Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<bool>(type: "bit", nullable: false),
                    IncorrectAnswer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestions");
        }
    }
}
