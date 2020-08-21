using Microsoft.EntityFrameworkCore.Migrations;

namespace WalkaboutProj.Migrations
{
    public partial class routeratingproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d23f8dcc-44ae-4a57-bf7c-26a0f4703590");

            migrationBuilder.AddColumn<int>(
                name: "RouteRating",
                table: "Routes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7612a40e-fc43-43c2-b120-6feb0624e3b4", "a60f6e47-1043-40bd-af31-128da16ca612", "Wanderer", "WANDERER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7612a40e-fc43-43c2-b120-6feb0624e3b4");

            migrationBuilder.DropColumn(
                name: "RouteRating",
                table: "Routes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d23f8dcc-44ae-4a57-bf7c-26a0f4703590", "defe8144-683c-4f36-9271-3de34392c3b1", "Wanderer", "WANDERER" });
        }
    }
}
