using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreCrafter.Infra.Sqllite.Migrations
{
    /// <inheritdoc />
    public partial class fix_user_fkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Grades_UserGradeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserGradeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserGradeId",
                table: "Users");

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserGrades",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentUserGradeId1",
                table: "Users",
                column: "CurrentUserGradeId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGrades_CurrentUserGradeId1",
                table: "Users",
                column: "CurrentUserGradeId1",
                principalTable: "UserGrades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGrades_Users_UserId1",
                table: "UserGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGrades_CurrentUserGradeId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentUserGradeId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserGrades_UserId1",
                table: "UserGrades");

            migrationBuilder.DropColumn(
                name: "CurrentUserGradeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentUserGradeId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserGrades");

            migrationBuilder.AddColumn<int>(
                name: "UserGradeId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGradeId",
                table: "Users",
                column: "UserGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Grades_UserGradeId",
                table: "Users",
                column: "UserGradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
