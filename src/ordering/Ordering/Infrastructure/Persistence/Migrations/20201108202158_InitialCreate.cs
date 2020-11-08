using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: true),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerEmail_Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerEmail_Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPhone_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobAddress_AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_Building = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_CountryRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaterialUnit = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MaterialQuantity = table.Column<double>(type: "float", nullable: false),
                    TruckType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TruckQuantity = table.Column<int>(type: "int", nullable: false),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeRate = table.Column<decimal>(type: "decimal(12,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Routes_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    RouteId = table.Column<long>(type: "bigint", nullable: false),
                    LoadLocation = table.Column<Point>(type: "geography", nullable: false),
                    LoadAddress_AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_Building = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_CountryRegion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpLocation = table.Column<Point>(type: "geography", nullable: false),
                    DumpAddress_AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_Building = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_CountryRegion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DumpAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DumpTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Legs_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legs_RouteId",
                table: "Legs",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_OrderItemId",
                table: "Routes",
                column: "OrderItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Legs");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
