using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationDevelopmentCourseProject.Migrations
{
    public partial class phoneandemailaddedtpuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");
        }
    }
}
