using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class hospitalNoRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalNo",
                table: "Locations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalNo",
                table: "Locations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
