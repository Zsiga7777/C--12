using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChineseKreta.Database.Migrations
{
    /// <inheritdoc />
    public partial class subjectstudentconnectionadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentEntitySubjectEntity",
                columns: table => new
                {
                    StudentsEducationalID = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SubjectsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEntitySubjectEntity", x => new { x.StudentsEducationalID, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_StudentEntitySubjectEntity_Student_StudentsEducationalID",
                        column: x => x.StudentsEducationalID,
                        principalTable: "Student",
                        principalColumn: "EducationalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentEntitySubjectEntity_Subject_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentEntitySubjectEntity_SubjectsId",
                table: "StudentEntitySubjectEntity",
                column: "SubjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentEntitySubjectEntity");
        }
    }
}
