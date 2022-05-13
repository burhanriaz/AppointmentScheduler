
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppointmentScheduling.Migrations
{
    public partial class UpdatecolumNameDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "Appointments",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointments",
                newName: "Discription");
        }
    }
}
