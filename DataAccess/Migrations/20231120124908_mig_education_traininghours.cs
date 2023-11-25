using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class mig_education_traininghours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingHoursId",
                table: "educations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_educations_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId",
                principalTable: "trainingHours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
