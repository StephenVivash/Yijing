using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SessionDb.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Sessions",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                YijingCast = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sessions", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Sessions",
            columns: new[] { "Id", "Description", "Name", "YijingCast" },
            values: new object[,]
            {
                { 1, "Morning meditation focusing on hexagram insight.", "Dawn Study", "Hexagram 24" },
                { 2, "Evening reflections guided by the I Ching.", "Lunar Notes", "Hexagram 15" },
                { 3, "Group reading exploring seasonal change.", "Oracle Circle", "Hexagram 49" }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Sessions");
    }
}
