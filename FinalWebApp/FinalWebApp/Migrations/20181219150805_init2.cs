using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalWebApp.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contry_City_CityId",
                table: "Contry");

            migrationBuilder.DropIndex(
                name: "IX_Contry_CityId",
                table: "Contry");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Contry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutDate",
                table: "Order",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInDate",
                table: "Order",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Hotel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContryId",
                table: "City",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_HotelId",
                table: "Order",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotel_CityId",
                table: "Hotel",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ContryId",
                table: "City",
                column: "ContryId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Contry_ContryId",
                table: "City",
                column: "ContryId",
                principalTable: "Contry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_City_CityId",
                table: "Hotel",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Hotel_HotelId",
                table: "Order",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Contry_ContryId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_City_CityId",
                table: "Hotel");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Hotel_HotelId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_HotelId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Hotel_CityId",
                table: "Hotel");

            migrationBuilder.DropIndex(
                name: "IX_City_ContryId",
                table: "City");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "ContryId",
                table: "City");

            migrationBuilder.AlterColumn<string>(
                name: "CheckOutDate",
                table: "Order",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "CheckInDate",
                table: "Order",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Contry",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contry_CityId",
                table: "Contry",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contry_City_CityId",
                table: "Contry",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
