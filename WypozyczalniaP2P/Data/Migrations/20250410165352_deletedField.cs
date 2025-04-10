using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WypozyczalniaP2P.Data.Migrations
{
    /// <inheritdoc />
    public partial class deletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CzyDostepny",
                table: "Samochody");

            migrationBuilder.AddColumn<string>(
                name: "Zdjecie",
                table: "Ogłoszenia",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zdjecie",
                table: "Ogłoszenia");

            migrationBuilder.AddColumn<bool>(
                name: "CzyDostepny",
                table: "Samochody",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 1,
                column: "CzyDostepny",
                value: true);

            migrationBuilder.UpdateData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 2,
                column: "CzyDostepny",
                value: true);

            migrationBuilder.UpdateData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 3,
                column: "CzyDostepny",
                value: true);
        }
    }
}
