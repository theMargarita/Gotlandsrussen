using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gotlandsrussen.Migrations
{
    /// <inheritdoc />
    public partial class NewServiceModelAndNewSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreakfastOptions");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingServices_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoomId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoomId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoomId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoomId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoomId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookedToDate", "NumberOfAdults" },
                values: new object[] { new DateOnly(2025, 6, 11), 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 6, 10), new DateOnly(2025, 6, 15), 1, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BookedFromDate", "BookedToDate", "BookingIsCancelled" },
                values: new object[] { new DateOnly(2025, 6, 11), new DateOnly(2025, 6, 13), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 6, 12), new DateOnly(2025, 6, 13), 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 6, 15), new DateOnly(2025, 6, 17), 2, 0 });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookedFromDate", "BookedToDate", "BookingIsCancelled", "GuestId", "NumberOfAdults", "NumberOfChildren" },
                values: new object[,]
                {
                    { 16, new DateOnly(2025, 8, 3), new DateOnly(2025, 8, 5), false, 1, 2, 4 },
                    { 17, new DateOnly(2025, 8, 5), new DateOnly(2025, 8, 7), false, 2, 4, 0 },
                    { 18, new DateOnly(2025, 8, 10), new DateOnly(2025, 8, 13), false, 3, 3, 3 },
                    { 19, new DateOnly(2025, 8, 12), new DateOnly(2025, 8, 13), false, 4, 4, 8 },
                    { 20, new DateOnly(2025, 8, 15), new DateOnly(2025, 8, 20), false, 5, 2, 5 }
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
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
                values: new object[] { 5, "Stables", 12, 5500m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoomTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoomTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoomTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoomTypeId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "RoomName", "RoomTypeId" },
                values: new object[,]
                {
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
                    { 24, "124", 4 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "AdultBreakfast", 100m },
                    { 2, "ChildBreakfast", 50m }
                });

            migrationBuilder.InsertData(
                table: "BookingRooms",
                columns: new[] { "Id", "BookingId", "RoomId" },
                values: new object[,]
                {
                    { 16, 16, 23 },
                    { 17, 17, 24 },
                    { 18, 18, 21 }
                });

            migrationBuilder.InsertData(
                table: "BookingServices",
                columns: new[] { "Id", "BookingId", "Quantity", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 4, 2, 1 },
                    { 10, 16, 2, 1 },
                    { 11, 16, 4, 2 },
                    { 12, 2, 1, 1 },
                    { 13, 3, 1, 1 },
                    { 14, 20, 2, 1 },
                    { 15, 20, 5, 2 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookedFromDate", "BookedToDate", "BookingIsCancelled", "GuestId", "NumberOfAdults", "NumberOfChildren" },
                values: new object[,]
                {
                    { 6, new DateOnly(2025, 6, 15), new DateOnly(2025, 6, 16), false, 6, 2, 0 },
                    { 7, new DateOnly(2025, 6, 16), new DateOnly(2025, 6, 18), false, 7, 2, 0 },
                    { 8, new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 2), false, 8, 2, 2 },
                    { 9, new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 3), false, 9, 2, 1 },
                    { 10, new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 5), false, 10, 2, 2 },
                    { 11, new DateOnly(2025, 7, 5), new DateOnly(2025, 7, 8), false, 11, 2, 2 },
                    { 12, new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 15), false, 12, 2, 1 },
                    { 13, new DateOnly(2025, 7, 17), new DateOnly(2025, 7, 19), false, 13, 2, 2 },
                    { 14, new DateOnly(2025, 7, 25), new DateOnly(2025, 7, 26), false, 14, 2, 3 },
                    { 15, new DateOnly(2025, 8, 2), new DateOnly(2025, 8, 3), false, 15, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "RoomName", "RoomTypeId" },
                values: new object[] { 25, "125", 5 });

            migrationBuilder.InsertData(
                table: "BookingRooms",
                columns: new[] { "Id", "BookingId", "RoomId" },
                values: new object[,]
                {
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
                    { 19, 19, 25 },
                    { 20, 20, 25 }
                });

            migrationBuilder.InsertData(
                table: "BookingServices",
                columns: new[] { "Id", "BookingId", "Quantity", "ServiceId" },
                values: new object[,]
                {
                    { 3, 7, 2, 1 },
                    { 4, 10, 2, 1 },
                    { 5, 10, 2, 2 },
                    { 6, 12, 2, 1 },
                    { 7, 12, 1, 2 },
                    { 8, 15, 2, 1 },
                    { 9, 15, 4, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_BookingId",
                table: "BookingServices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_ServiceId",
                table: "BookingServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoomId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoomId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoomId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoomId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BookingRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoomId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BookedToDate", "NumberOfAdults" },
                values: new object[] { new DateOnly(2025, 6, 15), 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 5), 2, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BookedFromDate", "BookedToDate", "BookingIsCancelled" },
                values: new object[] { new DateOnly(2025, 8, 20), new DateOnly(2025, 8, 25), true });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 9, 5), new DateOnly(2025, 9, 10), 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BookedFromDate", "BookedToDate", "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { new DateOnly(2025, 10, 15), new DateOnly(2025, 10, 18), 1, 1 });

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

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoomTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoomTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoomTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoomTypeId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_BreakfastOptions_BookingId",
                table: "BreakfastOptions",
                column: "BookingId",
                unique: true);
        }
    }
}
