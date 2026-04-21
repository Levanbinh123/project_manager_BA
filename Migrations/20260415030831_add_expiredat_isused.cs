using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_manager_api.Migrations
{
    /// <inheritdoc />
    public partial class add_expiredat_isused : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "Invitations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Invitations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Invitations");
        }
    }
}
