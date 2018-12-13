using Microsoft.EntityFrameworkCore.Migrations;

namespace PimpMyRide.Core.Data.Migrations
{
    public partial class numberOfCarsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfCars",
                table: "Manufacturers",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfCars",
                table: "Manufacturers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
