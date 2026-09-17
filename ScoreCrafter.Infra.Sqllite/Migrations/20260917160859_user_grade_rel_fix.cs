using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreCrafter.Infra.Sqllite.Migrations
{
    /// <inheritdoc />
    public partial class user_grade_rel_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGrades_CurrentUserGradeId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentUserGradeId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentUserGradeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentUserGradeId1",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentUserGradeId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentUserGradeId1",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentUserGradeId1",
                table: "Users",
                column: "CurrentUserGradeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGrades_CurrentUserGradeId1",
                table: "Users",
                column: "CurrentUserGradeId1",
                principalTable: "UserGrades",
                principalColumn: "Id");
        }
    }
}
