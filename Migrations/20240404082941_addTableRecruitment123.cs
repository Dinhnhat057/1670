using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungCore.Migrations
{
    /// <inheritdoc />
    public partial class addTableRecruitment123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recruitments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TinTuyenDungId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recruitments_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Recruitments_TinTuyenDungs_TinTuyenDungId",
                        column: x => x.TinTuyenDungId,
                        principalTable: "TinTuyenDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 15, 29, 40, 817, DateTimeKind.Local).AddTicks(6711));

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_AccountId",
                table: "Recruitments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_TinTuyenDungId",
                table: "Recruitments",
                column: "TinTuyenDungId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recruitments");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 15, 26, 3, 241, DateTimeKind.Local).AddTicks(8091));
        }
    }
}
