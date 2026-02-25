using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShelltyBlog.Migrations
{
    /// <inheritdoc />
    public partial class SeedBlogPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Category", "Content", "CreatedDate", "ModifiedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Behavior", "Cats have an inexplicable love for boxes of all sizes. Whether it's a tiny shoebox or a large cardboard container, if it fits, they sits! Scientists believe this behavior is rooted in their instinct to seek out confined spaces for safety and comfort. Boxes provide cats with a sense of security and a perfect spot for ambushing unsuspecting prey (or your ankles).", new DateTime(2026, 1, 9, 4, 34, 23, 78, DateTimeKind.Local).AddTicks(9102), null, "Why Cats Love Boxes" },
                    { 2, "Lifestyle", "Ever wonder what your cat does all day while you're at work? Indoor cats have their own daily routines that might surprise you. From patrolling their territory to taking strategic naps in sunny spots, cats are busy creatures. They spend about 70% of their lives sleeping, which means a 9-year-old cat has been awake for only three years of its life!", new DateTime(2026, 1, 14, 4, 34, 23, 78, DateTimeKind.Local).AddTicks(9181), null, "The Secret Life of Indoor Cats" },
                    { 3, "Behavior", "Cats communicate in many ways beyond meowing. They use body language, purring, and even slow blinks to express themselves. A slow blink from your cat is actually a sign of trust and affection - it's like a kitty kiss! Tail position, ear orientation, and whisker placement all tell a story about how your cat is feeling.", new DateTime(2026, 1, 17, 4, 34, 23, 78, DateTimeKind.Local).AddTicks(9251), null, "Understanding Cat Communication" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
