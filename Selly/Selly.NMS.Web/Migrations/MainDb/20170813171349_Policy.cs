using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Selly.NMS.Web.Migrations.MainDb
{
    public partial class Policy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyRules",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    Direction = table.Column<int>(nullable: false),
                    LocalAddress = table.Column<string>(nullable: true),
                    LocalPort = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PolicyId = table.Column<string>(nullable: true),
                    Protocol = table.Column<int>(nullable: false),
                    RemoteAddress = table.Column<string>(nullable: true),
                    RemotePort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyRules_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRules_PolicyId",
                table: "PolicyRules",
                column: "PolicyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyRules");

            migrationBuilder.DropTable(
                name: "Policies");
        }
    }
}
