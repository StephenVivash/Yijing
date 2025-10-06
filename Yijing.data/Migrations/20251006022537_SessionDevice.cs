using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yijing.data.Migrations
{
    /// <inheritdoc />
    public partial class SessionDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Muse",
                table: "Sessions",
                newName: "EegDevice");

            migrationBuilder.RenameColumn(
                name: "Emotiv",
                table: "Sessions",
                newName: "EegAnalysis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EegDevice",
                table: "Sessions",
                newName: "Muse");

            migrationBuilder.RenameColumn(
                name: "EegAnalysis",
                table: "Sessions",
                newName: "Emotiv");
        }
    }
}
