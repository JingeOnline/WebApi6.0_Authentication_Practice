using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbServiceLib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Student",
                columns: table => new
                {
                    PkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<bool>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Student", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Subject",
                columns: table => new
                {
                    PkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Subject", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Student_Subject",
                columns: table => new
                {
                    StudentsPkId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectsPkId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Student_Subject", x => new { x.StudentsPkId, x.SubjectsPkId });
                    table.ForeignKey(
                        name: "FK_tb_Student_Subject_tb_Student_StudentsPkId",
                        column: x => x.StudentsPkId,
                        principalTable: "tb_Student",
                        principalColumn: "PkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_Student_Subject_tb_Subject_SubjectsPkId",
                        column: x => x.SubjectsPkId,
                        principalTable: "tb_Subject",
                        principalColumn: "PkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Student_Subject_SubjectsPkId",
                table: "tb_Student_Subject",
                column: "SubjectsPkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_Student_Subject");

            migrationBuilder.DropTable(
                name: "tb_Student");

            migrationBuilder.DropTable(
                name: "tb_Subject");
        }
    }
}
