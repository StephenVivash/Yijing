using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YijingDb.Migrations
{
	public partial class CreateDB : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
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

			migrationBuilder.CreateTable(
				name: "Types",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false),
					Size = table.Column<int>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Types", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "LabelSeries",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_LabelSeries", x => x.Id);
					table.ForeignKey(
						name: "FK_LabelSeries_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RatioSeries",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RatioSeries", x => x.Id);
					table.ForeignKey(
						name: "FK_RatioSeries_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "SequenceSeries",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SequenceSeries", x => x.Id);
					table.ForeignKey(
						name: "FK_SequenceSeries_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "TextSeries",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					Name = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TextSeries", x => x.Id);
					table.ForeignKey(
						name: "FK_TextSeries_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Values",
				columns: table => new
				{
					TypeId = table.Column<int>(nullable: false),
					ValueId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Values", x => new { x.TypeId, x.ValueId });
					table.ForeignKey(
						name: "FK_Values_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Labels",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					ValueId = table.Column<int>(nullable: false),
					LabelSeryId = table.Column<int>(nullable: false),
					LabelData = table.Column<string>(maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Labels", x => x.Id);
					table.ForeignKey(
						name: "FK_Labels_LabelSeries_LabelSeryId",
						column: x => x.LabelSeryId,
						principalTable: "LabelSeries",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Labels_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Labels_Values_TypeId_ValueId",
						columns: x => new { x.TypeId, x.ValueId },
						principalTable: "Values",
						principalColumns: new[] { "TypeId", "ValueId" },
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Ratios",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					ValueId = table.Column<int>(nullable: false),
					RatioSeryId = table.Column<int>(nullable: false),
					RatioData = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Ratios", x => x.Id);
					table.ForeignKey(
						name: "FK_Ratios_RatioSeries_RatioSeryId",
						column: x => x.RatioSeryId,
						principalTable: "RatioSeries",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Ratios_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Ratios_Values_TypeId_ValueId",
						columns: x => new { x.TypeId, x.ValueId },
						principalTable: "Values",
						principalColumns: new[] { "TypeId", "ValueId" },
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Sequences",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					ValueId = table.Column<int>(nullable: false),
					SequenceSeryId = table.Column<int>(nullable: false),
					SequenceData = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Sequences", x => x.Id);
					table.ForeignKey(
						name: "FK_Sequences_SequenceSeries_SequenceSeryId",
						column: x => x.SequenceSeryId,
						principalTable: "SequenceSeries",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Sequences_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Sequences_Values_TypeId_ValueId",
						columns: x => new { x.TypeId, x.ValueId },
						principalTable: "Values",
						principalColumns: new[] { "TypeId", "ValueId" },
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Texts",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					TypeId = table.Column<int>(nullable: false),
					ValueId = table.Column<int>(nullable: false),
					TextSeryId = table.Column<int>(nullable: false),
					TextTypeId = table.Column<int>(nullable: false),
					TextData = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Texts", x => x.Id);
					table.ForeignKey(
						name: "FK_Texts_TextSeries_TextSeryId",
						column: x => x.TextSeryId,
						principalTable: "TextSeries",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Texts_TextTypes_TextTypeId",
						column: x => x.TextTypeId,
						principalTable: "TextTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Texts_Types_TypeId",
						column: x => x.TypeId,
						principalTable: "Types",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Texts_Values_TypeId_ValueId",
						columns: x => new { x.TypeId, x.ValueId },
						principalTable: "Values",
						principalColumns: new[] { "TypeId", "ValueId" },
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Labels_LabelSeryId",
				table: "Labels",
				column: "LabelSeryId");

			migrationBuilder.CreateIndex(
				name: "IX_Labels_TypeId_ValueId",
				table: "Labels",
				columns: new[] { "TypeId", "ValueId" });

			migrationBuilder.CreateIndex(
				name: "IX_LabelSeries_TypeId",
				table: "LabelSeries",
				column: "TypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Ratios_RatioSeryId",
				table: "Ratios",
				column: "RatioSeryId");

			migrationBuilder.CreateIndex(
				name: "IX_Ratios_TypeId_ValueId",
				table: "Ratios",
				columns: new[] { "TypeId", "ValueId" });

			migrationBuilder.CreateIndex(
				name: "IX_RatioSeries_TypeId",
				table: "RatioSeries",
				column: "TypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Sequences_SequenceSeryId",
				table: "Sequences",
				column: "SequenceSeryId");

			migrationBuilder.CreateIndex(
				name: "IX_Sequences_TypeId_ValueId",
				table: "Sequences",
				columns: new[] { "TypeId", "ValueId" });

			migrationBuilder.CreateIndex(
				name: "IX_SequenceSeries_TypeId",
				table: "SequenceSeries",
				column: "TypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Texts_TextSeryId",
				table: "Texts",
				column: "TextSeryId");

			migrationBuilder.CreateIndex(
				name: "IX_Texts_TextTypeId",
				table: "Texts",
				column: "TextTypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Texts_TypeId_ValueId",
				table: "Texts",
				columns: new[] { "TypeId", "ValueId" });

			migrationBuilder.CreateIndex(
				name: "IX_TextSeries_TypeId",
				table: "TextSeries",
				column: "TypeId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Labels");

			migrationBuilder.DropTable(
				name: "Ratios");

			migrationBuilder.DropTable(
				name: "Sequences");

			migrationBuilder.DropTable(
				name: "Texts");

			migrationBuilder.DropTable(
				name: "LabelSeries");

			migrationBuilder.DropTable(
				name: "RatioSeries");

			migrationBuilder.DropTable(
				name: "SequenceSeries");

			migrationBuilder.DropTable(
				name: "TextSeries");

			migrationBuilder.DropTable(
				name: "TextTypes");

			migrationBuilder.DropTable(
				name: "Values");

			migrationBuilder.DropTable(
				name: "Types");
		}
	}
}
