using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungCore.Migrations
{
    /// <inheritdoc />
    public partial class updateTableNhaTuyendung_202403311058 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "NhaTuyenDungs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NhaTuyenDungs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "NhaTuyenDungs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 31, 10, 58, 41, 26, DateTimeKind.Local).AddTicks(7482));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "NhaTuyenDungs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NhaTuyenDungs");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "NhaTuyenDungs");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 31, 10, 18, 6, 273, DateTimeKind.Local).AddTicks(9847));
        }
    }
}
