using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yijing.data.Migrations
{
    /// <inheritdoc />
    public partial class AddMeditation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meditations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meditations", x => x.Id);
                });

            var seedData = GenerateMeditationSeedData();
            foreach (var meditation in seedData)
            {
                migrationBuilder.InsertData(
                    table: "Meditations",
                    columns: new[] { "Id", "Start", "Duration" },
                    values: new object[] { meditation.Id, meditation.Start, meditation.Duration });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meditations");
        }

        private static (int Id, DateTime Start, int Duration)[] GenerateMeditationSeedData()
        {
            var now = DateTime.UtcNow;
            var startDate = now.AddMonths(-6).Date;
            var random = new Random(20251121);
            var id = 1;
            var data = new List<(int Id, DateTime Start, int Duration)>();

            for (var day = startDate; day <= now.Date; day = day.AddDays(random.Next(1, 3)))
            {
                var startTime = day.AddHours(random.Next(5, 23)).AddMinutes(random.Next(0, 60));
                var duration = random.Next(30, 121);
                data.Add((id++, startTime, duration));
            }

            return data.ToArray();
        }
    }
}
