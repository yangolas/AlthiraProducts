using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlthiraProducts.Adapters.Repository.Write.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDepartmentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeparmentName",
                table: "Categories",
                newName: "DepartmentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Categories",
                newName: "DeparmentName");
        }
    }
}
