using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AIMAS.API.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alerttimeinventory_inventory_InventoryID",
                table: "alerttimeinventory");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "reservation");

            migrationBuilder.AlterColumn<long>(
                name: "InventoryID",
                table: "alerttimeinventory",
                type: "int8",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_alerttimeinventory_inventory_InventoryID",
                table: "alerttimeinventory",
                column: "InventoryID",
                principalTable: "inventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alerttimeinventory_inventory_InventoryID",
                table: "alerttimeinventory");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "reservation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "InventoryID",
                table: "alerttimeinventory",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "int8");

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AlertDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    InventoryID = table.Column<long>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    UpcomingEventDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_notification_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_InventoryID",
                table: "notification",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_notification_UserId",
                table: "notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_alerttimeinventory_inventory_InventoryID",
                table: "alerttimeinventory",
                column: "InventoryID",
                principalTable: "inventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
