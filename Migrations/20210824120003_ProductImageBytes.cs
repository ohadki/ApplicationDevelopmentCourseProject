using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationDevelopmentCourseProject.Migrations
{
    public partial class ProductImageBytes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Product",
                type: "varbinary(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
