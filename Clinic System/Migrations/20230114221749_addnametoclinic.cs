using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic_System.Migrations
{
    public partial class addnametoclinic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clinics",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clinics");
        }
    }
}
