using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shellty_Blog.Migrations
{
    /// <inheritdoc />
    public partial class StoreImagesInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "ImageFileName",
                table: "BlogPosts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "BlogPosts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "BlogPosts",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "ImageFileName",
                table: "BlogPosts",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Category", "Content", "CreatedDate", "ImageFileName", "ModifiedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Information", "I created this small CMS system for an engineering academy. Here you can find current information about my activities.", new DateTime(2026, 2, 28, 12, 0, 0, 0, DateTimeKind.Utc), null, null, "Welcome to my shell blog! " },
                    { 2, "Question", "Ever wonder what your cat does all day while you're at work? Indoor cats have their own daily routines that might surprise you. From patrolling their territory to taking strategic naps in sunny spots, cats are busy creatures. They spend about 70% of their lives sleeping, which means a 9-year-old cat has been awake for only three years of its life!", new DateTime(2025, 1, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, null, "The Secret Life of Indoor Cats" },
                    { 3, "Behavior", "Cats communicate in many ways beyond meowing. They use body language, purring, and even slow blinks to express themselves. A slow blink from your cat is actually a sign of trust and affection - it's like a kitty kiss! Tail position, ear orientation, and whisker placement all tell a story about how your cat is feeling.", new DateTime(2025, 1, 17, 12, 0, 0, 0, DateTimeKind.Utc), null, null, "Understanding Cat Communication" }
                });
        }
    }
}
