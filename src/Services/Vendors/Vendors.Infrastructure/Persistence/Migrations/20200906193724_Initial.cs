using Microsoft.EntityFrameworkCore.Migrations;

namespace Vendors.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MinorityTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinorityTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Name = table.Column<string>(nullable: false),
                    PrimaryAddress_AddressLine1 = table.Column<string>(nullable: true),
                    PrimaryAddress_AddressLine2 = table.Column<string>(nullable: true),
                    PrimaryAddress_Building = table.Column<string>(nullable: true),
                    PrimaryAddress_City = table.Column<string>(nullable: true),
                    PrimaryAddress_StateProvince = table.Column<string>(nullable: true),
                    PrimaryAddress_CountryRegion = table.Column<string>(nullable: true),
                    PrimaryAddress_PostalCode = table.Column<string>(nullable: true),
                    AlternateAddress_AddressLine1 = table.Column<string>(nullable: true),
                    AlternateAddress_AddressLine2 = table.Column<string>(nullable: true),
                    AlternateAddress_Building = table.Column<string>(nullable: true),
                    AlternateAddress_City = table.Column<string>(nullable: true),
                    AlternateAddress_StateProvince = table.Column<string>(nullable: true),
                    AlternateAddress_CountryRegion = table.Column<string>(nullable: true),
                    AlternateAddress_PostalCode = table.Column<string>(nullable: true),
                    PhoneNumber_Number = table.Column<string>(nullable: true),
                    FaxNumber_Number = table.Column<string>(nullable: true),
                    WebAddress = table.Column<string>(nullable: false),
                    RegionId = table.Column<long>(nullable: true),
                    CommunicationMethod = table.Column<int>(nullable: false),
                    ProfileLogoUrl = table.Column<string>(nullable: false),
                    ProfileAccentColor = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Profiles_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Name = table.Column<string>(nullable: false),
                    PrimaryAddress_AddressLine1 = table.Column<string>(nullable: true),
                    PrimaryAddress_AddressLine2 = table.Column<string>(nullable: true),
                    PrimaryAddress_Building = table.Column<string>(nullable: true),
                    PrimaryAddress_City = table.Column<string>(nullable: true),
                    PrimaryAddress_StateProvince = table.Column<string>(nullable: true),
                    PrimaryAddress_CountryRegion = table.Column<string>(nullable: true),
                    PrimaryAddress_PostalCode = table.Column<string>(nullable: true),
                    AlternateAddress_AddressLine1 = table.Column<string>(nullable: true),
                    AlternateAddress_AddressLine2 = table.Column<string>(nullable: true),
                    AlternateAddress_Building = table.Column<string>(nullable: true),
                    AlternateAddress_City = table.Column<string>(nullable: true),
                    AlternateAddress_StateProvince = table.Column<string>(nullable: true),
                    AlternateAddress_CountryRegion = table.Column<string>(nullable: true),
                    AlternateAddress_PostalCode = table.Column<string>(nullable: true),
                    PhoneNumber_Number = table.Column<string>(nullable: true),
                    FaxNumber_Number = table.Column<string>(nullable: true),
                    WebAddress = table.Column<string>(nullable: false),
                    RegionId = table.Column<long>(nullable: true),
                    CommunicationMethod = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    TypeId = table.Column<long>(nullable: true),
                    IsBonded = table.Column<bool>(nullable: false),
                    BondRate = table.Column<decimal>(type: "decimal(10, 6)", nullable: false),
                    Note = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Vendors_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendors_CompanyTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileContacts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    PhoneNumber_Number = table.Column<string>(nullable: true),
                    FaxNumber_Number = table.Column<string>(nullable: true),
                    CellPhoneNumber_Number = table.Column<string>(nullable: true),
                    EmailAddress_Identifier = table.Column<string>(nullable: true),
                    EmailAddress_Domain = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: false),
                    IsMainContact = table.Column<bool>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileContacts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ProfileContacts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileMinorityStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    TypeId = table.Column<long>(nullable: false),
                    CertificationNumber = table.Column<string>(nullable: false),
                    Percent = table.Column<decimal>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileMinorityStatuses", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ProfileMinorityStatuses_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileMinorityStatuses_MinorityTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MinorityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    TypeId = table.Column<long>(nullable: false),
                    RegionId = table.Column<long>(nullable: true),
                    ProfileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileProducts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ProfileProducts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileProducts_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileProducts_ProductTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorContacts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    PhoneNumber_Number = table.Column<string>(nullable: true),
                    FaxNumber_Number = table.Column<string>(nullable: true),
                    CellPhoneNumber_Number = table.Column<string>(nullable: true),
                    EmailAddress_Identifier = table.Column<string>(nullable: true),
                    EmailAddress_Domain = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: false),
                    IsMainContact = table.Column<bool>(nullable: false),
                    VendorId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContacts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_VendorContacts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorMinorityStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    TypeId = table.Column<long>(nullable: false),
                    CertificationNumber = table.Column<string>(nullable: false),
                    Percent = table.Column<decimal>(nullable: false),
                    VendorId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorMinorityStatuses", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_VendorMinorityStatuses_MinorityTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MinorityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorMinorityStatuses_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    TypeId = table.Column<long>(nullable: false),
                    RegionId = table.Column<long>(nullable: true),
                    VendorId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorProducts", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_VendorProducts_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendorProducts_ProductTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorProducts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileContacts_ProfileId",
                table: "ProfileContacts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMinorityStatuses_ProfileId",
                table: "ProfileMinorityStatuses",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMinorityStatuses_TypeId",
                table: "ProfileMinorityStatuses",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileProducts_ProfileId",
                table: "ProfileProducts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileProducts_RegionId",
                table: "ProfileProducts",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileProducts_TypeId",
                table: "ProfileProducts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RegionId",
                table: "Profiles",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_VendorId",
                table: "VendorContacts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorMinorityStatuses_TypeId",
                table: "VendorMinorityStatuses",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorMinorityStatuses_VendorId",
                table: "VendorMinorityStatuses",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProducts_RegionId",
                table: "VendorProducts",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProducts_TypeId",
                table: "VendorProducts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProducts_VendorId",
                table: "VendorProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_RegionId",
                table: "Vendors",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_TypeId",
                table: "Vendors",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileContacts");

            migrationBuilder.DropTable(
                name: "ProfileMinorityStatuses");

            migrationBuilder.DropTable(
                name: "ProfileProducts");

            migrationBuilder.DropTable(
                name: "VendorContacts");

            migrationBuilder.DropTable(
                name: "VendorMinorityStatuses");

            migrationBuilder.DropTable(
                name: "VendorProducts");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "MinorityTypes");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "CompanyTypes");
        }
    }
}
