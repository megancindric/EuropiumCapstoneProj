using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WalkaboutProj.Migrations
{
    public partial class datetimeroutechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ea3930-faff-40c6-9c34-febfbbbf8875");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "LocationLat",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "LocationLong",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Routes");

            migrationBuilder.AddColumn<float>(
                name: "TotalTimeMilliseconds",
                table: "Routes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d23f8dcc-44ae-4a57-bf7c-26a0f4703590", "defe8144-683c-4f36-9271-3de34392c3b1", "Wanderer", "WANDERER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d23f8dcc-44ae-4a57-bf7c-26a0f4703590");

            migrationBuilder.DropColumn(
                name: "TotalTimeMilliseconds",
                table: "Routes");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "LocationLat",
                table: "Routes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LocationLong",
                table: "Routes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "32ea3930-faff-40c6-9c34-febfbbbf8875", "13e3ccf2-417f-424d-b3e5-8f11809c9834", "Wanderer", "WANDERER" });
        }
    }
}
