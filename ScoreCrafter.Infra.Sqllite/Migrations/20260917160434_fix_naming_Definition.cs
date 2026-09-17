using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreCrafter.Infra.Sqllite.Migrations
{
    /// <inheritdoc />
    public partial class fix_naming_Definition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Grades_GradeId",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_GradeId_Version",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Formulas");

            migrationBuilder.RenameColumn(
                name: "Recipe",
                table: "Formulas",
                newName: "Definition");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Definition",
                table: "Formulas",
                newName: "Recipe");

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Formulas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_GradeId_Version",
                table: "Formulas",
                columns: new[] { "GradeId", "Version" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Grades_GradeId",
                table: "Formulas",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
