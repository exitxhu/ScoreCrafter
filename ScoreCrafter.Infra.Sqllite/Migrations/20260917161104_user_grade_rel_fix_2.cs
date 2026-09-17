using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreCrafter.Infra.Sqllite.Migrations
{
    /// <inheritdoc />
    public partial class user_grade_rel_fix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGrades_Users_UserId1",
                table: "UserGrades");

            migrationBuilder.DropIndex(
                name: "IX_UserGrades_UserId1",
                table: "UserGrades");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserGrades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserGrades",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGrades_UserId1",
                table: "UserGrades",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGrades_Users_UserId1",
                table: "UserGrades",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
