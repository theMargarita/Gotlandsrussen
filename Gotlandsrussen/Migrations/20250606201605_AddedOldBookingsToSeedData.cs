using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GotlandsrussenAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedOldBookingsToSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "Breakfast", "FromDate", "GuestId", "IsCancelled", "NumberOfAdults", "NumberOfChildren", "ToDate" },
                values: new object[,]
                {
                    { 21, true, new DateOnly(2025, 5, 1), 1, false, 2, 1, new DateOnly(2025, 5, 17) },
                    { 22, false, new DateOnly(2025, 4, 20), 2, false, 1, 2, new DateOnly(2025, 4, 22) },
                    { 23, true, new DateOnly(2025, 1, 5), 3, false, 1, 0, new DateOnly(2025, 6, 6) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
