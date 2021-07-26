using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "vendors",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Locations",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "LocationId");

            migrationBuilder.CreateTable(
                name: "Class_Item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    LocationId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_Item_Locations_LocationId1",
                        column: x => x.LocationId1,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Class_Item_LocationId1",
                table: "Class_Item",
                column: "LocationId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Class_Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "vendors",
                table: "Locations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");
        }
    }
}
