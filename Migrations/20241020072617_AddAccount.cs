using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coursemvc.Migrations
{
    /// <inheritdoc />
    public partial class AddAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Courses",
                newName: "IsAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAccount",
                table: "Courses",
                newName: "IsActive");
        }
    }
}
