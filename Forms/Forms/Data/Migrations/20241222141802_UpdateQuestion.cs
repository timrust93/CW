using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forms.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QCheckbox1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckbox2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckbox3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckbox4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckboxState1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckboxState2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckboxState3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QCheckboxState4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QInt1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QInt2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QInt3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QInt4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiString1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiString2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiString3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiString4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiStringState1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiStringState2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiStringState3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QMultiStringState4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QString1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QString2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QString3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QString4",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QStringState1",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QStringState2",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QStringState3",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QStringState4",
                table: "Templates");

            migrationBuilder.RenameColumn(
                name: "QIntState4",
                table: "Templates",
                newName: "SingleLineQLimit");

            migrationBuilder.RenameColumn(
                name: "QIntState3",
                table: "Templates",
                newName: "NumberQLimit");

            migrationBuilder.RenameColumn(
                name: "QIntState2",
                table: "Templates",
                newName: "MultilineQLimint");

            migrationBuilder.RenameColumn(
                name: "QIntState1",
                table: "Templates",
                newName: "CheckboxQLimit");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_TemplateId",
                table: "Question",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.RenameColumn(
                name: "SingleLineQLimit",
                table: "Templates",
                newName: "QIntState4");

            migrationBuilder.RenameColumn(
                name: "NumberQLimit",
                table: "Templates",
                newName: "QIntState3");

            migrationBuilder.RenameColumn(
                name: "MultilineQLimint",
                table: "Templates",
                newName: "QIntState2");

            migrationBuilder.RenameColumn(
                name: "CheckboxQLimit",
                table: "Templates",
                newName: "QIntState1");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Templates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "QCheckbox1",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckbox2",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckbox3",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckbox4",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckboxState1",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckboxState2",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckboxState3",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QCheckboxState4",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QInt1",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QInt2",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QInt3",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QInt4",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QMultiString1",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiString2",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiString3",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiString4",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiStringState1",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiStringState2",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiStringState3",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QMultiStringState4",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QString1",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QString2",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QString3",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QString4",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QStringState1",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QStringState2",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QStringState3",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QStringState4",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
