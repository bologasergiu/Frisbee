using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrisbeeApp.Context.Migrations
{
    public partial class AddAccountApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccountApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountApproved",
                table: "AspNetUsers");
        }
    }
}
