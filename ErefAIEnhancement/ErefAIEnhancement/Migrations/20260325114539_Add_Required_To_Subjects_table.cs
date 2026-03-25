using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErefAIEnhancement.Migrations
{
    /// <inheritdoc />
    public partial class Add_Required_To_Subjects_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "Subjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Required",
                table: "Subjects");
        }
    }
}
