using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Characters", "Description", "Exits", "Monsters", "Name" },
                values: new object[] { new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "[]", "Welcome to the Town Square. You see a fountain in the center of the square.", "North,South,East,West", "[\"1888A3D0-F507-4901-AD1D-0E163CDF60A9\",\"1888A3D0-F507-4901-AD1D-0E163CDF60A9\"]", "Town Square" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"));
        }
    }
}
