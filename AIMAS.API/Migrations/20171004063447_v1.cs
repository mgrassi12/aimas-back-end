using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AIMAS.API.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.UniqueConstraint("AK_AspNetRoles_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccessFailedCount = table.Column<int>(type: "int4", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bool", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bool", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bool", nullable: false),
                    Position = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bool", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.UniqueConstraint("AK_AspNetUsers_Email", x => x.Email);
                    table.UniqueConstraint("AK_AspNetUsers_UserName", x => x.UserName);
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
                name: "location",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "int8", nullable: false),
                    RoleId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "int8", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                name: "inventory",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CurrentLocationID = table.Column<long>(type: "int8", nullable: false),
                    DefaultLocationID = table.Column<long>(type: "int8", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    IsArchived = table.Column<bool>(type: "bool", nullable: false),
                    IsCritical = table.Column<bool>(type: "bool", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    MaintenanceIntervalDays = table.Column<long>(type: "int8", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_inventory_location_CurrentLocationID",
                        column: x => x.CurrentLocationID,
                        principalTable: "location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_location_DefaultLocationID",
                        column: x => x.DefaultLocationID,
                        principalTable: "location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BookingEnd = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    BookingPurpose = table.Column<string>(type: "text", nullable: false),
                    BookingStart = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    IsArchived = table.Column<bool>(type: "bool", nullable: false),
                    LocationID = table.Column<long>(type: "int8", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_reservation_location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alertTime",
                columns: table => new
                {
                    ID = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DaysBefore = table.Column<long>(type: "int8", nullable: false),
                    InventoryID = table.Column<long>(type: "int8", nullable: false),
                    Type = table.Column<int>(type: "int4", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_alertTime_InventoryID",
                table: "alertTime",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "IX_inventory_CurrentLocationID",
                table: "inventory",
                column: "CurrentLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_DefaultLocationID",
                table: "inventory",
                column: "DefaultLocationID");

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
                name: "IX_reservation_LocationID",
                table: "reservation",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_UserId",
                table: "reservation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reservationIdentity_InventoryID",
                table: "reservationIdentity",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_timeLog_UserId",
                table: "timeLog",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alertTime");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
