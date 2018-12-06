using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PimpMyRide.Core.Data.Migrations
{
    public partial class usermodeltypofix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_EngingeTypes_EngineTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EngingeTypes",
                table: "EngingeTypes");

            migrationBuilder.RenameTable(
                name: "EngingeTypes",
                newName: "EngineTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EngineTypes",
                table: "EngineTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_EngineTypes_EngineTypeId",
                table: "Cars",
                column: "EngineTypeId",
                principalTable: "EngineTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_EngineTypes_EngineTypeId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EngineTypes",
                table: "EngineTypes");

            migrationBuilder.RenameTable(
                name: "EngineTypes",
                newName: "EngingeTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EngingeTypes",
                table: "EngingeTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_EngingeTypes_EngineTypeId",
                table: "Cars",
                column: "EngineTypeId",
                principalTable: "EngingeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
