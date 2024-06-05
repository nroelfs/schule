using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAppSchule.Migrations
{
    /// <inheritdoc />
    public partial class Add_ThemeDetection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFirstLoginDone",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFirstLoginDone",
                table: "AspNetUsers");
        }
    }
}
