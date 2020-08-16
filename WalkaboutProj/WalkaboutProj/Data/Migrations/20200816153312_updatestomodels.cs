using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WalkaboutProj.Data.Migrations
{
    public partial class updatestomodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7377a85d-2824-4727-a69b-f264224afcd1");

            migrationBuilder.CreateTable(
                name: "Markers",
                columns: table => new
                {
                    MarkerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(nullable: false),
                    MarkerName = table.Column<string>(nullable: true),
                    MarkerCategory = table.Column<string>(nullable: true),
                    PicturePath = table.Column<string>(nullable: true),
                    MarkerDescription = table.Column<string>(nullable: true),
                    IsFavorite = table.Column<bool>(nullable: false),
                    PointValue = table.Column<double>(nullable: false),
                    MarkerLat = table.Column<double>(nullable: false),
                    MarkerLong = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markers", x => x.MarkerId);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WandererId = table.Column<int>(nullable: false),
                    RouteName = table.Column<string>(nullable: false),
                    RouteDescription = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TotalDistance = table.Column<double>(nullable: false),
                    TotalPoints = table.Column<double>(nullable: false),
                    LocationLat = table.Column<double>(nullable: false),
                    LocationLong = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                });

            migrationBuilder.CreateTable(
                name: "Wanderers",
                columns: table => new
                {
                    WandererId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    UnitPreference = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wanderers", x => x.WandererId);
                    table.ForeignKey(
                        name: "FK_Wanderers_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b946729f-a795-4360-84d6-cbaf97d76790", "7495abec-26d3-4da1-93e7-e8b46204315d", "Wanderer", "WANDERER" });

            migrationBuilder.CreateIndex(
                name: "IX_Wanderers_IdentityUserId",
                table: "Wanderers",
                column: "IdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Markers");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Wanderers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b946729f-a795-4360-84d6-cbaf97d76790");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7377a85d-2824-4727-a69b-f264224afcd1", "40a5eed1-e7d1-43be-ada8-8f84987e4f93", "Wanderer", "WANDERER" });
        }
    }
}
