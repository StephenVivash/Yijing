using System;
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
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Meditations");
		}
	}
}
