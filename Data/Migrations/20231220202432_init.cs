using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_Manager.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "Date", "Name", "Paid", "Quantity", "Status", "TotalPrice" },
                values: new object[] { 1, new DateTime(2023, 7, 23, 20, 24, 32, 327, DateTimeKind.Utc).AddTicks(8077), "Google Pixel Phone", true, 23, "OutForDelivery", 9600 });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "Date", "Name", "Paid", "Quantity", "Status", "TotalPrice" },
                values: new object[] { 2, new DateTime(2023, 9, 11, 20, 24, 32, 327, DateTimeKind.Utc).AddTicks(8076), "Microsoft Surface", true, 5, "Arrived", 2600 });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "Date", "Name", "Paid", "Quantity", "Status", "TotalPrice" },
                values: new object[] { 3, new DateTime(2023, 10, 31, 20, 24, 32, 327, DateTimeKind.Utc).AddTicks(8067), "Apple Watch Series 4", true, 10, "Arrived", 1600 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
