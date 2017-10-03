﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AIMAS.API.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_location_LocationID",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_reservation_inventory_InventoryID",
                table: "reservation");

            migrationBuilder.DropIndex(
                name: "IX_reservation_InventoryID",
                table: "reservation");

            migrationBuilder.DropIndex(
                name: "IX_inventory_LocationID",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "InventoryID",
                table: "reservation");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "inventory");

            migrationBuilder.RenameColumn(
                name: "MaintanceDate",
                table: "inventory",
                newName: "MaintenanceDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingStart",
                table: "reservation",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingEnd",
                table: "reservation",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "BookingPurpose",
                table: "reservation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "reservation",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LocationID",
                table: "reservation",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CurrentLocationID",
                table: "inventory",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DefaultLocationID",
                table: "inventory",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "inventory",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCritical",
                table: "inventory",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "MaintenanceIntervalNumber",
                table: "inventory",
                type: "int8",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceIntervalType",
                table: "inventory",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "alertTime",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DaysBefore = table.Column<long>(type: "int8", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alertTime", x => x.ID);
                    table.ForeignKey(
                        name: "FK_alertTime_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "changeEvent",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ChangeTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ChangeType = table.Column<string>(type: "text", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false),
                    NewValue = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changeEvent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_changeEvent_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_changeEvent_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AlertDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UpcomingEventDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "report",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CreatorId = table.Column<long>(type: "int8", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ExecutorId = table.Column<long>(type: "int8", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.ID);
                    table.ForeignKey(
                        name: "FK_report_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_report_AspNetUsers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_report_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservationIdentity",
                columns: table => new
                {
                    ReservationID = table.Column<long>(type: "int8", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservationIdentity", x => new { x.ReservationID, x.InventoryID });
                    table.ForeignKey(
                        name: "FK_reservationIdentity_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservationIdentity_reservation_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "reservation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "timeLog",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CheckIn = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CheckInLodged = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CheckOutLodged = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    Purpose = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_timeLog_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categoryIdentity",
                columns: table => new
                {
                    CategoryID = table.Column<long>(type: "int8", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoryIdentity", x => new { x.CategoryID, x.InventoryID });
                    table.ForeignKey(
                        name: "FK_categoryIdentity_category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_categoryIdentity_inventory_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservation_LocationID",
                table: "reservation",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_CurrentLocationID",
                table: "inventory",
                column: "CurrentLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_DefaultLocationID",
                table: "inventory",
                column: "DefaultLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_alertTime_InventoryID",
                table: "alertTime",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_categoryIdentity_InventoryID",
                table: "categoryIdentity",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_changeEvent_InventoryID",
                table: "changeEvent",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_changeEvent_UserId",
                table: "changeEvent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_InventoryID",
                table: "notification",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_notification_UserId",
                table: "notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_report_CreatorId",
                table: "report",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_report_ExecutorId",
                table: "report",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_report_InventoryID",
                table: "report",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_reservationIdentity_InventoryID",
                table: "reservationIdentity",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_timeLog_UserId",
                table: "timeLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_location_CurrentLocationID",
                table: "inventory",
                column: "CurrentLocationID",
                principalTable: "location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_location_DefaultLocationID",
                table: "inventory",
                column: "DefaultLocationID",
                principalTable: "location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reservation_location_LocationID",
                table: "reservation",
                column: "LocationID",
                principalTable: "location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_location_CurrentLocationID",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_location_DefaultLocationID",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_reservation_location_LocationID",
                table: "reservation");

            migrationBuilder.DropTable(
                name: "alertTime");

            migrationBuilder.DropTable(
                name: "categoryIdentity");

            migrationBuilder.DropTable(
                name: "changeEvent");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "report");

            migrationBuilder.DropTable(
                name: "reservationIdentity");

            migrationBuilder.DropTable(
                name: "timeLog");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropIndex(
                name: "IX_reservation_LocationID",
                table: "reservation");

            migrationBuilder.DropIndex(
                name: "IX_inventory_CurrentLocationID",
                table: "inventory");

            migrationBuilder.DropIndex(
                name: "IX_inventory_DefaultLocationID",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "BookingPurpose",
                table: "reservation");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "reservation");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "reservation");

            migrationBuilder.DropColumn(
                name: "CurrentLocationID",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "DefaultLocationID",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "IsCritical",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "MaintenanceIntervalNumber",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "MaintenanceIntervalType",
                table: "inventory");

            migrationBuilder.RenameColumn(
                name: "MaintenanceDate",
                table: "inventory",
                newName: "MaintanceDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingStart",
                table: "reservation",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingEnd",
                table: "reservation",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AddColumn<long>(
                name: "InventoryID",
                table: "reservation",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LocationID",
                table: "inventory",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_reservation_InventoryID",
                table: "reservation",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_LocationID",
                table: "inventory",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_location_LocationID",
                table: "inventory",
                column: "LocationID",
                principalTable: "location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservation_inventory_InventoryID",
                table: "reservation",
                column: "InventoryID",
                principalTable: "inventory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
