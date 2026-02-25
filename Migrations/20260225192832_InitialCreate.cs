using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shellty_Blog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Category", "Content", "CreatedDate", "ModifiedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Behavior", "Cats have an inexplicable love for boxes of all sizes. Whether it's a tiny shoebox or a large cardboard container, if it fits, they sits! Scientists believe this behavior is rooted in their instinct to seek out confined spaces for safety and comfort. Boxes provide cats with a sense of security and a perfect spot for ambushing unsuspecting prey (or your ankles).", new DateTime(2025, 1, 9, 12, 0, 0, 0, DateTimeKind.Utc), null, "Why Cats Love Boxes" },
                    { 2, "Lifestyle", "Ever wonder what your cat does all day while you're at work? Indoor cats have their own daily routines that might surprise you. From patrolling their territory to taking strategic naps in sunny spots, cats are busy creatures. They spend about 70% of their lives sleeping, which means a 9-year-old cat has been awake for only three years of its life!", new DateTime(2025, 1, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, "The Secret Life of Indoor Cats" },
                    { 3, "Behavior", "Cats communicate in many ways beyond meowing. They use body language, purring, and even slow blinks to express themselves. A slow blink from your cat is actually a sign of trust and affection - it's like a kitty kiss! Tail position, ear orientation, and whisker placement all tell a story about how your cat is feeling.", new DateTime(2025, 1, 17, 12, 0, 0, 0, DateTimeKind.Utc), null, "Understanding Cat Communication" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");
        }
    }
}
