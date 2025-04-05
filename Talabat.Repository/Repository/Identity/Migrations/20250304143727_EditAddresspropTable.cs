using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Repository.Identity.Migrations
{
    public partial class EditAddresspropTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LName",
                table: "Addresses",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FName",
                table: "Addresses",
                newName: "FirstNName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Addresses",
                newName: "LName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Addresses",
                newName: "FName");
        }
    }
}
