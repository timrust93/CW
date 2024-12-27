using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACheckbox1",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ACheckbox2",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ACheckbox3",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ACheckbox4",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AInt1",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AInt2",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AInt3",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AInt4",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AMultiString1",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AMultiString2",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AMultiString3",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AMultiString4",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AString1",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AString2",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "AString3",
                table: "Forms");

            migrationBuilder.RenameColumn(
                name: "AString4",
                table: "Forms",
                newName: "Author");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Templates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Question",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Forms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    IntAnswer = table.Column<int>(type: "int", nullable: false),
                    BoolAnswer = table.Column<bool>(type: "bit", nullable: false),
                    StringAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Forms_FormsId",
                        column: x => x.FormsId,
                        principalTable: "Forms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_FormsId",
                table: "Answer",
                column: "FormsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Forms");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Forms",
                newName: "AString4");

            migrationBuilder.AddColumn<bool>(
                name: "ACheckbox1",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ACheckbox2",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ACheckbox3",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ACheckbox4",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AInt1",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AInt2",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AInt3",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AInt4",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AMultiString1",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AMultiString2",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AMultiString3",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AMultiString4",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AString1",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AString2",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AString3",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
