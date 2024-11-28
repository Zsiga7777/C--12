using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChineseKreta.Database.Migrations
{
    /// <inheritdoc />
    public partial class CityModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Street_City_CityPostalCode",
                table: "Street");

            migrationBuilder.DropIndex(
                name: "IX_Street_CityPostalCode",
                table: "Street");

            migrationBuilder.DropColumn(
                name: "CityPostalCode",
                table: "Street");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Street",
                newName: "CityId");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "City",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Street_CityId",
                table: "Street",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Street_City_CityId",
                table: "Street",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Street_City_CityId",
                table: "Street");

            migrationBuilder.DropIndex(
                name: "IX_Street_CityId",
                table: "Street");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Street",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "City",
                newName: "PostalCode");

            migrationBuilder.AddColumn<long>(
                name: "CityPostalCode",
                table: "Street",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Street_CityPostalCode",
                table: "Street",
                column: "CityPostalCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Street_City_CityPostalCode",
                table: "Street",
                column: "CityPostalCode",
                principalTable: "City",
                principalColumn: "PostalCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
