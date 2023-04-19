using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDesigner.Migrations
{
    public partial class troll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9be2b4b-7e00-435a-b970-d9882d7bb69d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e38aff69-1fc3-459f-83af-7ceef123c7f8");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Order",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78f77ed3-9ca4-4387-afee-1b92c5fa96aa", "115aead9-e6ae-4969-b7f0-027ce0eefe07", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a20516e6-2731-4fea-8a71-e122d42901ff", "7e5e4fe3-e8ec-4136-abf2-dc53d874f26a", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78f77ed3-9ca4-4387-afee-1b92c5fa96aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a20516e6-2731-4fea-8a71-e122d42901ff");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d9be2b4b-7e00-435a-b970-d9882d7bb69d", "17006e13-d659-4658-8754-8521b64c97d7", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e38aff69-1fc3-459f-83af-7ceef123c7f8", "2c9f9a61-5482-464c-960c-26914d408fcf", "Administrator", "ADMINISTRATOR" });
        }
    }
}
