using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GotlandsrussenAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "integer", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuestId = table.Column<int>(type: "integer", nullable: false),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfAdults = table.Column<int>(type: "integer", nullable: false),
                    NumberOfChildren = table.Column<int>(type: "integer", nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    Breakfast = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RoomTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice", "Andersson", "0701234567" },
                    { 2, "bob@example.com", "Bob", "Bengtsson", "0702345678" },
                    { 3, "tom@example.com", "Tom", "Larsson", "0702345679" },
                    { 4, "lisa@example.com", "Lisa", "Helgesson", "0702345680" },
                    { 5, "kalle@example.com", "Kalle", "Spongtsson", "0702345681" },
                    { 6, "emma@example.com", "Emma", "Nilsson", "0702345682" },
                    { 7, "oscar@example.com", "Oscar", "Johansson", "0702345683" },
                    { 8, "sara@example.com", "Sara", "Karlsson", "0702345684" },
                    { 9, "erik@example.com", "Erik", "Svensson", "0702345685" },
                    { 10, "maja@example.com", "Maja", "Gustafsson", "0702345686" },
                    { 11, "johan@example.com", "Johan", "Lindberg", "0702345687" },
                    { 12, "elin@example.com", "Elin", "Persson", "0702345688" },
                    { 13, "andreas@example.com", "Andreas", "Eriksson", "0702345689" },
                    { 14, "nora@example.com", "Nora", "Bergström", "0702345690" },
                    { 15, "viktor@example.com", "Viktor", "Holm", "0702345691" }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[,]
                {
                    { 1, "Single", 1, 500m },
                    { 2, "Double", 2, 900m },
                    { 3, "Family", 4, 1500m },
                    { 4, "Suite", 6, 3000m },
                    { 5, "Stables", 12, 5500m }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "Breakfast", "FromDate", "GuestId", "IsCancelled", "NumberOfAdults", "NumberOfChildren", "ToDate" },
                values: new object[,]
                {
                    { 1, false, new DateOnly(2025, 6, 10), 1, false, 1, 0, new DateOnly(2025, 6, 11) },
                    { 2, true, new DateOnly(2025, 6, 10), 2, false, 1, 0, new DateOnly(2025, 6, 15) },
                    { 3, false, new DateOnly(2025, 6, 11), 3, false, 1, 0, new DateOnly(2025, 6, 13) },
                    { 4, true, new DateOnly(2025, 6, 12), 4, false, 2, 0, new DateOnly(2025, 6, 13) },
                    { 5, false, new DateOnly(2025, 6, 15), 5, false, 2, 0, new DateOnly(2025, 6, 17) },
                    { 6, true, new DateOnly(2025, 6, 15), 6, false, 2, 0, new DateOnly(2025, 6, 16) },
                    { 7, true, new DateOnly(2025, 6, 16), 7, false, 2, 0, new DateOnly(2025, 6, 18) },
                    { 8, true, new DateOnly(2025, 7, 1), 8, false, 2, 2, new DateOnly(2025, 7, 2) },
                    { 9, false, new DateOnly(2025, 7, 1), 9, false, 2, 1, new DateOnly(2025, 7, 3) },
                    { 10, false, new DateOnly(2025, 7, 1), 10, false, 2, 2, new DateOnly(2025, 7, 5) },
                    { 11, true, new DateOnly(2025, 7, 5), 11, false, 2, 2, new DateOnly(2025, 7, 8) },
                    { 12, true, new DateOnly(2025, 7, 10), 12, false, 2, 1, new DateOnly(2025, 7, 15) },
                    { 13, true, new DateOnly(2025, 7, 17), 13, false, 2, 2, new DateOnly(2025, 7, 19) },
                    { 14, false, new DateOnly(2025, 7, 25), 14, false, 2, 3, new DateOnly(2025, 7, 26) },
                    { 15, false, new DateOnly(2025, 8, 2), 15, false, 2, 4, new DateOnly(2025, 8, 3) },
                    { 16, false, new DateOnly(2025, 8, 3), 1, false, 2, 4, new DateOnly(2025, 8, 5) },
                    { 17, false, new DateOnly(2025, 8, 5), 2, false, 4, 0, new DateOnly(2025, 8, 7) },
                    { 18, false, new DateOnly(2025, 8, 10), 3, false, 3, 3, new DateOnly(2025, 8, 13) },
                    { 19, false, new DateOnly(2025, 8, 12), 4, false, 4, 8, new DateOnly(2025, 8, 13) },
                    { 20, true, new DateOnly(2025, 8, 15), 5, false, 2, 5, new DateOnly(2025, 8, 20) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "RoomTypeId" },
                values: new object[,]
                {
                    { 1, "101", 1 },
                    { 2, "102", 1 },
                    { 3, "103", 1 },
                    { 4, "104", 1 },
                    { 5, "105", 1 },
                    { 6, "106", 2 },
                    { 7, "107", 2 },
                    { 8, "108", 2 },
                    { 9, "109", 2 },
                    { 10, "110", 2 },
                    { 11, "111", 3 },
                    { 12, "112", 3 },
                    { 13, "113", 3 },
                    { 14, "114", 3 },
                    { 15, "115", 3 },
                    { 16, "116", 3 },
                    { 17, "117", 3 },
                    { 18, "118", 3 },
                    { 19, "119", 3 },
                    { 20, "120", 3 },
                    { 21, "121", 4 },
                    { 22, "122", 4 },
                    { 23, "123", 4 },
                    { 24, "124", 4 },
                    { 25, "125", 5 }
                });

            migrationBuilder.InsertData(
                table: "BookingRooms",
                columns: new[] { "Id", "BookingId", "RoomId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 6 },
                    { 5, 5, 7 },
                    { 6, 6, 8 },
                    { 7, 7, 9 },
                    { 8, 8, 11 },
                    { 9, 9, 12 },
                    { 10, 10, 13 },
                    { 11, 11, 14 },
                    { 12, 12, 15 },
                    { 13, 13, 16 },
                    { 14, 14, 21 },
                    { 15, 15, 22 },
                    { 16, 16, 23 },
                    { 17, 17, 24 },
                    { 18, 18, 21 },
                    { 19, 19, 25 },
                    { 20, 20, 25 }
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
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingRooms");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "RoomTypes");
        }
    }
}
