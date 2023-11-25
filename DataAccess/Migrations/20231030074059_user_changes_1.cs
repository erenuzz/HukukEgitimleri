using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class user_changes_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_educations_EducationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "EducationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_educations_EducationId",
                table: "AspNetUsers",
                column: "EducationId",
                principalTable: "educations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_educations_EducationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "EducationId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_educations_EducationId",
                table: "AspNetUsers",
                column: "EducationId",
                principalTable: "educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
