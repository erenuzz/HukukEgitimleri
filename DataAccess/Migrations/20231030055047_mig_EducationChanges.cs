using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class mig_EducationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_educations_trainers_TrainerId",
                table: "educations");

            migrationBuilder.DropIndex(
                name: "IX_educations_TrainerId",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "Quota",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "TotalCourseHours",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "educations");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "educations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "educations");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "educations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Quota",
                table: "educations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCourseHours",
                table: "educations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "educations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_educations_TrainerId",
                table: "educations",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_educations_trainers_TrainerId",
                table: "educations",
                column: "TrainerId",
                principalTable: "trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
