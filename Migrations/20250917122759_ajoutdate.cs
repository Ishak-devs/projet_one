using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_one.Migrations
{
    /// <inheritdoc />
    public partial class ajoutdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDemande",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDemande",
                table: "AspNetUsers");
        }
    }
}
