using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic_System.Migrations
{
    public partial class addinitialdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Genders",
               columns: new[] { "Name" },
               values: new object[,] {
                {"ذكر" },
                {"انثي" },
                {"اخر" }
               }
               );
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Name" },
                values: new object[,] {
                {"مصر" },
                {"السعودية" },
                {"ليبيا" }
                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Countries");
        }
    }
}
