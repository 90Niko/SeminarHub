using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier of the category.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of the category.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "This class represents a category.");

            migrationBuilder.CreateTable(
                name: "Seminars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier of the seminar.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The topic of the seminar."),
                    Lecturer = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, comment: "The lecturer of the seminar."),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "The details of the seminar."),
                    OrganizerId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "The organizer identifier of the seminar."),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time of the seminar."),
                    Duration = table.Column<int>(type: "int", maxLength: 180, nullable: false, comment: "The duration of the seminar in minutes."),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "The Category identifier of the seminar.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seminars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seminars_AspNetUsers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seminars_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "This class represents a seminar.");

            migrationBuilder.CreateTable(
                name: "SeminarsParticipants",
                columns: table => new
                {
                    SeminarId = table.Column<int>(type: "int", nullable: false, comment: "The seminar identifier."),
                    ParticipantId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "The participant identifier.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeminarsParticipants", x => new { x.SeminarId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_SeminarsParticipants_AspNetUsers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeminarsParticipants_Seminars_SeminarId",
                        column: x => x.SeminarId,
                        principalTable: "Seminars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "This class represents a seminar participant.");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Technology & Innovation" },
                    { 2, "Business & Entrepreneurship" },
                    { 3, "Science & Research" },
                    { 4, "Arts & Culture" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_CategoryId",
                table: "Seminars",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_OrganizerId",
                table: "Seminars",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarsParticipants_ParticipantId",
                table: "SeminarsParticipants",
                column: "ParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeminarsParticipants");

            migrationBuilder.DropTable(
                name: "Seminars");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
