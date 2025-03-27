using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WypozyczalniaP2P.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TypySamochodow",
                columns: new[] { "Id", "Nazwa", "Opis" },
                values: new object[,]
                {
                    { 1, "Sedan", "Rodzinna limuzyna" },
                    { 2, "SUV", "Duże auto" },
                    { 3, "Hatchback", "Miejskie auto" },
                    { 4, "Combi", "Długie auto" }
                });

            migrationBuilder.InsertData(
                table: "Samochody",
                columns: new[] { "Id", "CzyDostepny", "IloscDrzwi", "Kolor", "LiczbaMiejsc", "Marka", "MocSilnika", "Model", "NumerRejestracyjny", "PojemnoscSilnika", "Przebieg", "RodzajNapedu", "RodzajPaliwa", "RokProdukcji", "Skrzynia", "TypSamochoduId", "Vin", "WlascicielId", "Zdjecie" },
                values: new object[,]
                {
                    { 1, true, 1, 1, 2, "Toyota", 132, "Corolla", "WWA1234", 2f, 45000, 0, 0, 2019, 1, 1, "JTDBR32E042013579", "85b4c380-cb76-41ff-9387-eed59e4040a2", "default.jpg" },
                    { 2, true, 2, 7, 2, "Volkswagen", 150, "Tiguan", "WRO5678", 3f, 30000, 2, 1, 2020, 0, 2, "WVGZZZ5NZLW123456", "85b4c380-cb76-41ff-9387-eed59e4040a2", "default.jpg" },
                    { 3, true, 2, 2, 2, "Ford", 100, "Fiesta", "KRA9012", 1f, 60000, 0, 0, 2018, 0, 3, "WF0DXXGAJD1234567", "85b4c380-cb76-41ff-9387-eed59e4040a2", "default.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Samochody",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TypySamochodow",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TypySamochodow",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TypySamochodow",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TypySamochodow",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
