using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class mig_education_traininghours_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingHoursId",
                table: "educations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId",
                principalTable: "trainingHours",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingHoursId",
                table: "educations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_educations_trainingHours_TrainingHoursId",
                table: "educations",
                column: "TrainingHoursId",
                principalTable: "trainingHours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
