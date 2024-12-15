using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forms.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    AString1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AString2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AString3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AString4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AInt1 = table.Column<int>(type: "int", nullable: false),
                    AInt2 = table.Column<int>(type: "int", nullable: false),
                    AInt3 = table.Column<int>(type: "int", nullable: false),
                    AInt4 = table.Column<int>(type: "int", nullable: false),
                    AMultiString1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMultiString2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMultiString3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMultiString4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ACheckbox1 = table.Column<bool>(type: "bit", nullable: false),
                    ACheckbox2 = table.Column<bool>(type: "bit", nullable: false),
                    ACheckbox3 = table.Column<bool>(type: "bit", nullable: false),
                    ACheckbox4 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QStringState1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QString1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QStringState2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QString2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QStringState3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QString3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QStringState4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QString4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiStringState1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiString1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiStringState2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiString2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiStringState3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiString3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiStringState4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QMultiString4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QCheckboxState1 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckbox1 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckboxState2 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckbox2 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckboxState3 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckbox3 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckboxState4 = table.Column<bool>(type: "bit", nullable: false),
                    QCheckbox4 = table.Column<bool>(type: "bit", nullable: false),
                    QIntState1 = table.Column<int>(type: "int", nullable: false),
                    QInt1 = table.Column<int>(type: "int", nullable: false),
                    QIntState2 = table.Column<int>(type: "int", nullable: false),
                    QInt2 = table.Column<int>(type: "int", nullable: false),
                    QIntState3 = table.Column<int>(type: "int", nullable: false),
                    QInt3 = table.Column<int>(type: "int", nullable: false),
                    QIntState4 = table.Column<int>(type: "int", nullable: false),
                    QInt4 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateAccess_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateAccess_TemplateId",
                table: "TemplateAccess",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "TemplateAccess");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
