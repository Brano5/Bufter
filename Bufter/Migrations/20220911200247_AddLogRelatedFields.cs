using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bufter.Migrations
{
    public partial class AddLogRelatedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "PcName",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "UniKey",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "BuyLog",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "PcName",
                table: "BuyLog",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "BuyLog",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "UniKey",
                table: "BuyLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Log");
            migrationBuilder.DropColumn(
                name: "PcName",
                table: "Log");
            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "BuyLLogog");
            migrationBuilder.DropColumn(
                name: "UniKey",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "BuyLog");
            migrationBuilder.DropColumn(
                name: "PcName",
                table: "BuyLog");
            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "BuyLog");
            migrationBuilder.DropColumn(
                name: "UniKey",
                table: "BuyLog");
        }
    }
}
