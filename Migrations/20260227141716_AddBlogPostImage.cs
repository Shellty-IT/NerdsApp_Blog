using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shellty_Blog.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "BlogPosts",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Content", "CreatedDate", "ImageFileName", "Title" },
                values: new object[] { "Information", "I created this small CMS system for an engineering academy. Here you can find current information about my activities.", new DateTime(2026, 2, 28, 12, 0, 0, 0, DateTimeKind.Utc), null, "Welcome to my shell blog! " });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "ImageFileName" },
                values: new object[] { "Question", null });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageFileName",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Content", "CreatedDate", "Title" },
                values: new object[] { "Behavior", "Cats have an inexplicable love for boxes of all sizes. Whether it's a tiny shoebox or a large cardboard container, if it fits, they sits! Scientists believe this behavior is rooted in their instinct to seek out confined spaces for safety and comfort. Boxes provide cats with a sense of security and a perfect spot for ambushing unsuspecting prey (or your ankles).", new DateTime(2025, 1, 9, 12, 0, 0, 0, DateTimeKind.Utc), "Why Cats Love Boxes" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: "Lifestyle");
        }
    }
}
