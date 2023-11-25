using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class mig_TrainingHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trainingHours_educations_EducationId",
                table: "trainingHours");

            migrationBuilder.DropIndex(
                name: "IX_trainingHours_EducationId",
                table: "trainingHours");

            migrationBuilder.DropColumn(
                name: "EducationId",
                table: "trainingHours");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "educations");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "trainingHours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "trainingHours");

            migrationBuilder.AddColumn<int>(
                name: "EducationId",
                table: "trainingHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "educations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_trainingHours_EducationId",
                table: "trainingHours",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_trainingHours_educations_EducationId",
                table: "trainingHours",
                column: "EducationId",
                principalTable: "educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
