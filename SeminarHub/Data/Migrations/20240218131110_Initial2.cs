using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Seminars",
                type: "int",
                maxLength: 180,
                nullable: true,
                comment: "The duration of the seminar in minutes.",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 180,
                oldComment: "The duration of the seminar in minutes.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Seminars",
                type: "int",
                maxLength: 180,
                nullable: false,
                defaultValue: 0,
                comment: "The duration of the seminar in minutes.",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 180,
                oldNullable: true,
                oldComment: "The duration of the seminar in minutes.");
        }
    }
}
