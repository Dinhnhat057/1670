using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungCore.Migrations
{
    /// <inheritdoc />
    public partial class InitSeeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedDate", "Email", "Password", "Role", "Status" },
                values: new object[] { -1, new DateTime(2024, 3, 31, 9, 8, 30, 455, DateTimeKind.Local).AddTicks(9784), "admin@gmail.com", "21232f297a57a5a743894a0e4a801fc3", 2, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
