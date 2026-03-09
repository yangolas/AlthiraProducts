using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlthiraProducts.Adapters.Repository.Write.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTraceContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProductImages",
                newName: "InsertedAt");

            migrationBuilder.AddColumn<string>(
                name: "TraceContext",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedAt",
                table: "OutboxEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TraceContext",
                table: "OutboxEvents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraceContext",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "InsertedAt",
                table: "OutboxEvents");

            migrationBuilder.DropColumn(
                name: "TraceContext",
                table: "OutboxEvents");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "ProductImages",
                newName: "CreatedAt");
        }
    }
}
