using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FrisbeeApp.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("79f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"), "1", "Player", "Player" },
                    { new Guid("89f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"), "2", "Coach", "Coach" },
                    { new Guid("99f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"), "3", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("79f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("89f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("99f4438c-5a0f-4ed2-aa0b-5d3bfb5de3a6"));
        }
    }
}
