using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class mig_changes_Edu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations");

            migrationBuilder.DropIndex(
                name: "IX_educations_TrainingHoursId",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "TrainingHoursId",
                table: "educations");

            migrationBuilder.AddColumn<int>(
                name: "EducationId",
                table: "trainingHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "TrainingHoursId",
                table: "educations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_educations_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId",
                principalTable: "trainingHours",
                principalColumn: "Id");
        }
    }
}
