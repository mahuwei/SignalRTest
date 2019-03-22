using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySqlTest.Migrations
{
    public partial class V001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RowFlag = table.Column<DateTime>(rowVersion: true, nullable: true),
                    LastChange = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(maxLength: 100, nullable: true),
                    No = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Businesses");
        }
    }
}
