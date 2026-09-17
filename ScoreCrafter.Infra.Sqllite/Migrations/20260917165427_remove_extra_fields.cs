using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreCrafter.Infra.Sqllite.Migrations
{
    /// <inheritdoc />
    public partial class remove_extra_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserScore",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UserScore",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
