using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrisbeeApp.Context.Migrations
{
    public partial class QuizQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncorrectAnswer",
                table: "QuizQuestions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncorrectAnswer",
                table: "QuizQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
