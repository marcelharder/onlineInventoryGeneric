using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class hospitalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naam = table.Column<string>(nullable: true),
                    Adres = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    HospitalNo = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    RefHospitals = table.Column<string>(nullable: true),
                    StandardRef = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Contact_image = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    mrnSample = table.Column<string>(nullable: true),
                    vendors = table.Column<string>(nullable: true),
                    rp = table.Column<string>(nullable: true),
                    SMS_mobile_number = table.Column<string>(nullable: true),
                    SMS_send_time = table.Column<string>(nullable: true),
                    triggerOneMonth = table.Column<bool>(nullable: false),
                    triggerTwoMonth = table.Column<bool>(nullable: false),
                    triggerThreeMonth = table.Column<bool>(nullable: false),
                    DBBackend = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Adres = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Contact = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Contact_image = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Country = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DBBackend = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Fax = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    HospitalNo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Image = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Logo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Naam = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PostalCode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RefHospitals = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SMS_mobile_number = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SMS_send_time = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StandardRef = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Telephone = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    mrnSample = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    rp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    triggerOneMonth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    triggerThreeMonth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    triggerTwoMonth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    vendors = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });
        }
    }
}
