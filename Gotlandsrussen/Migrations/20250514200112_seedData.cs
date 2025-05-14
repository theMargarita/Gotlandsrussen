using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gotlandsrussen.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Breakfast_BreakfastPriceId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "BookingRoom");

            migrationBuilder.DropTable(
                name: "Breakfast");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BreakfastPriceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Breakfast",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BreakfastPriceId",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "RoomTypes",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "BookingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingRooms_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreakfastOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    IsIncluded = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakfastOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreakfastOptions_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice", "Andersson", "0701234567" },
                    { 2, "bob@example.com", "Bob", "Bengtsson", "0702345678" },
                    { 3, "tom@example.com", "Tom", "Larsson", "0702345679" },
                    { 4, "lisa@example.com", "Lisa", "Helgesson", "0702345680" },
                    { 5, "kalle@example.com", "Kalle", "Spongtsson", "0702345681" }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[,]
                {
                    { 1, "Single", 1, 500m },
                    { 2, "Double", 2, 900m },
                    { 3, "Family", 4, 1500m },
                    { 4, "Suite", 6, 3000m }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookedFromDate", "BookedToDate", "BookingIsCancelled", "GuestId", "NumberOfAdults", "NumberOfChildren" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 6, 10), new DateOnly(2025, 6, 15), false, 1, 2, 0 },
                    { 2, new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 5), false, 2, 2, 2 },
                    { 3, new DateOnly(2025, 8, 20), new DateOnly(2025, 8, 25), true, 3, 1, 0 },
                    { 4, new DateOnly(2025, 9, 5), new DateOnly(2025, 9, 10), false, 4, 2, 1 },
                    { 5, new DateOnly(2025, 10, 15), new DateOnly(2025, 10, 18), false, 5, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "RoomName", "RoomTypeId" },
                values: new object[,]
                {
                    { 1, "101", 1 },
                    { 2, "102", 2 },
                    { 3, "103", 3 },
                    { 4, "104", 4 },
                    { 5, "105", 3 }
                });

            migrationBuilder.InsertData(
                table: "BookingRooms",
                columns: new[] { "Id", "BookingId", "RoomId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 2, 3 },
                    { 3, 3, 4 },
                    { 4, 4, 1 },
                    { 5, 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "BreakfastOptions",
                columns: new[] { "Id", "BookingId", "IsIncluded" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 2, 2, false },
                    { 3, 3, true },
                    { 4, 4, false },
                    { 5, 5, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRooms_BookingId",
                table: "BookingRooms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRooms_RoomId",
                table: "BookingRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BreakfastOptions_BookingId",
                table: "BreakfastOptions",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingRooms");

            migrationBuilder.DropTable(
                name: "BreakfastOptions");

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "RoomTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<bool>(
                name: "Breakfast",
                table: "Bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BreakfastPriceId",
                table: "Bookings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookingRoom",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoom", x => new { x.BookingId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_BookingRoom_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Breakfast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricePerAdult = table.Column<decimal>(type: "numeric", nullable: false),
                    PricePerChild = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breakfast", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BreakfastPriceId",
                table: "Bookings",
                column: "BreakfastPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_RoomId",
                table: "BookingRoom",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Breakfast_BreakfastPriceId",
                table: "Bookings",
                column: "BreakfastPriceId",
                principalTable: "Breakfast",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
