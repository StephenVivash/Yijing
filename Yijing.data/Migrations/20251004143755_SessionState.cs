using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yijing.data.Migrations
{
    /// <inheritdoc />
    public partial class SessionState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Emotiv",
                table: "Sessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Meditation",
                table: "Sessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Muse",
                table: "Sessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emotiv",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Meditation",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Muse",
                table: "Sessions");
        }
    }
}
