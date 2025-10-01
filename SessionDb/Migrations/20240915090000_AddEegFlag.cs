using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SessionDb.Migrations
{
    public partial class AddEegFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EEG",
                table: "Sessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EEG",
                table: "Sessions");
        }
    }
}
