using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forms.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckboxQLimit",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "MultilineQLimint",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "NumberQLimit",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SingleLineQLimit",
                table: "Templates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckboxQLimit",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MultilineQLimint",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberQLimit",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SingleLineQLimit",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
