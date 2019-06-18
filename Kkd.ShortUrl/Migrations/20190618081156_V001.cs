using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kkd.ShortUrl.Migrations
{
    public partial class V001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaxRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LastChange = table.Column<DateTime>(nullable: false),
                    RowFlag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    No = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaxRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlMaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LastChange = table.Column<DateTime>(nullable: false),
                    RowFlag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LongUrl = table.Column<string>(maxLength: 200, nullable: false),
                    ShortUrl = table.Column<string>(maxLength: 200, nullable: false),
                    Md5 = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LastChange = table.Column<DateTime>(nullable: false),
                    RowFlag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: false),
                    Token = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaxRecords");

            migrationBuilder.DropTable(
                name: "UrlMaps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
