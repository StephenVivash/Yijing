using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YijingDb.Migrations
{
	public partial class TextType : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "TextType",
				table: "Texts");

			migrationBuilder.AddColumn<int>(
				name: "TextTypeId",
				table: "Texts",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.CreateTable(
				name: "TextTypes",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TextTypes", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Texts_TextTypeId",
				table: "Texts",
				column: "TextTypeId");

			migrationBuilder.AddForeignKey(
				name: "FK_Texts_TextTypes_TextTypeId",
				table: "Texts",
				column: "TextTypeId",
				principalTable: "TextTypes",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Texts_TextTypes_TextTypeId",
				table: "Texts");

			migrationBuilder.DropTable(
				name: "TextTypes");

			migrationBuilder.DropIndex(
				name: "IX_Texts_TextTypeId",
				table: "Texts");

			migrationBuilder.DropColumn(
				name: "TextTypeId",
				table: "Texts");

			migrationBuilder.AddColumn<string>(
				name: "TextType",
				table: "Texts",
				maxLength: 100,
				nullable: false,
				defaultValue: "");
		}
	}
}
