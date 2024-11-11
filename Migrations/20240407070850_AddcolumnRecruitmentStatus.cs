using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungCore.Migrations
{
    /// <inheritdoc />
    public partial class AddcolumnRecruitmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Recruitments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 7, 14, 8, 47, 812, DateTimeKind.Local).AddTicks(9776));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Recruitments");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 15, 29, 40, 817, DateTimeKind.Local).AddTicks(6711));
        }
    }
}
