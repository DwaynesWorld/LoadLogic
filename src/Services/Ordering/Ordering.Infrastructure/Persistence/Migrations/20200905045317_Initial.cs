using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    OrderNo = table.Column<int>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: true),
                    IsDraft = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<long>(nullable: true),
                    CustomerName = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerEmail_Identifier = table.Column<string>(nullable: true),
                    CustomerEmail_Domain = table.Column<string>(nullable: true),
                    CustomerPhone_Number = table.Column<string>(nullable: true),
                    JobName = table.Column<string>(maxLength: 50, nullable: true),
                    JobDescription = table.Column<string>(maxLength: 200, nullable: true),
                    JobAddress_AddressLine1 = table.Column<string>(nullable: true),
                    JobAddress_AddressLine2 = table.Column<string>(nullable: true),
                    JobAddress_Building = table.Column<string>(nullable: true),
                    JobAddress_City = table.Column<string>(nullable: true),
                    JobAddress_StateProvince = table.Column<string>(nullable: true),
                    JobAddress_CountryRegion = table.Column<string>(nullable: true),
                    JobAddress_PostalCode = table.Column<string>(nullable: true),
                    JobStartDate = table.Column<DateTime>(nullable: true),
                    JobEndDate = table.Column<DateTime>(nullable: true)
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    OrderId = table.Column<long>(nullable: false),
                    Activity = table.Column<int>(nullable: false),
                    MaterialId = table.Column<long>(nullable: false),
                    MaterialName = table.Column<string>(maxLength: 50, nullable: false),
                    MaterialUnit = table.Column<string>(maxLength: 5, nullable: false),
                    MaterialQuantity = table.Column<double>(nullable: false),
                    TruckType = table.Column<string>(nullable: false),
                    TruckQuantity = table.Column<int>(nullable: false),
                    ChargeType = table.Column<string>(nullable: false),
                    ChargeRate = table.Column<decimal>(type: "decimal(12, 6)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
