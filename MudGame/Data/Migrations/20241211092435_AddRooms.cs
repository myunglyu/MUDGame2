using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Exits = table.Column<string>(type: "TEXT", nullable: true),
                    Monsters = table.Column<string>(type: "TEXT", nullable: true),
                    Characters = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Characters", "Description", "Exits", "Monsters", "Name" },
                values: new object[] { new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "[]", "Welcome to the Town Square. You see a fountain in the center of the square.", "None", "[\"1888A3D0-F507-4901-AD1D-0E163CDF60A9\",\"1888A3D0-F507-4901-AD1D-0E163CDF60A9\"]", "Town Square" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
