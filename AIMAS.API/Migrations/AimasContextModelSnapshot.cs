﻿// <auto-generated />
using AIMAS.Data;
using AIMAS.Data.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AIMAS.API.Migrations
{
    [DbContext(typeof(AimasContext))]
    partial class AimasContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("AIMAS.Data.Identity.RoleModel_DB", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name")
                        .HasName("AK_AspNetRoles_Name");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("AIMAS.Data.Identity.TimeLogModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("timestamptz");

                    b.Property<DateTime>("CheckInLodged")
                        .HasColumnType("timestamptz");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("timestamptz");

                    b.Property<DateTime>("CheckOutLodged")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Purpose")
                        .IsRequired();

                    b.Property<long>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("timeLog");
                });

            modelBuilder.Entity("AIMAS.Data.Identity.UserModel_DB", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasAlternateKey("Email")
                        .HasName("AK_AspNetUsers_Email");


                    b.HasAlternateKey("UserName")
                        .HasName("AK_AspNetUsers_UserName");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.AlertTimeInventoryModel_DB", b =>
                {
                    b.Property<long>("AlertID");

                    b.Property<long>("InventoryID");

                    b.Property<DateTime?>("SentTime")
                        .HasColumnType("timestamptz");

                    b.Property<int>("Type");

                    b.HasKey("AlertID", "InventoryID");

                    b.HasIndex("InventoryID");

                    b.ToTable("alerttimeinventory");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.AlertTimeModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DaysBefore");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("alertTime");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.CategoryInventoryModel_DB", b =>
                {
                    b.Property<long>("CategoryID");

                    b.Property<long>("InventoryID");

                    b.HasKey("CategoryID", "InventoryID");

                    b.HasIndex("InventoryID");

                    b.ToTable("categoryIdentity");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.CategoryModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.ToTable("category");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ChangeEventModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ChangeTime")
                        .HasColumnType("timestamptz");

                    b.Property<string>("ChangeType")
                        .IsRequired();

                    b.Property<long>("InventoryID");

                    b.Property<string>("NewValue")
                        .IsRequired();

                    b.Property<string>("OldValue")
                        .IsRequired();

                    b.Property<long>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("InventoryID");

                    b.HasIndex("UserId");

                    b.ToTable("changeEvent");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.InventoryModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CurrentLocationID");

                    b.Property<long?>("DefaultLocationID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamptz");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsCritical");

                    b.Property<long>("MaintenanceIntervalDays");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("CurrentLocationID");

                    b.HasIndex("DefaultLocationID");

                    b.ToTable("inventory");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.LocationModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("location");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.NotificationModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AlertDate")
                        .HasColumnType("timestamptz");

                    b.Property<long>("InventoryID");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<DateTime>("UpcomingEventDate")
                        .HasColumnType("timestamptz");

                    b.Property<long>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("InventoryID");

                    b.HasIndex("UserId");

                    b.ToTable("notification");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReportModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamptz");

                    b.Property<long>("CreatorId");

                    b.Property<DateTime>("ExecutionDate")
                        .HasColumnType("timestamptz");

                    b.Property<long>("ExecutorId");

                    b.Property<long>("InventoryID");

                    b.Property<string>("Notes");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ExecutorId");

                    b.HasIndex("InventoryID");

                    b.ToTable("report");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReservationInventoryModel_DB", b =>
                {
                    b.Property<long>("ReservationID");

                    b.Property<long>("InventoryID");

                    b.HasKey("ReservationID", "InventoryID");

                    b.HasIndex("InventoryID");

                    b.ToTable("reservationIdentity");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReservationModel_DB", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BookingEnd")
                        .HasColumnType("timestamptz");

                    b.Property<string>("BookingPurpose")
                        .IsRequired();

                    b.Property<DateTime>("BookingStart")
                        .HasColumnType("timestamptz");

                    b.Property<bool>("IsArchived");

                    b.Property<long>("LocationID");

                    b.Property<long>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("LocationID");

                    b.HasIndex("UserId");

                    b.ToTable("reservation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AIMAS.Data.Identity.TimeLogModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.AlertTimeInventoryModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.AlertTimeModel_DB", "AlertTime")
                        .WithMany()
                        .HasForeignKey("AlertID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany("AlertTimeInventories")
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.CategoryInventoryModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.CategoryModel_DB", "Category")
                        .WithMany("CategoryInventories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany("CategoryInventories")
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ChangeEventModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.InventoryModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.LocationModel_DB", "CurrentLocation")
                        .WithMany()
                        .HasForeignKey("CurrentLocationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Inventory.LocationModel_DB", "DefaultLocation")
                        .WithMany()
                        .HasForeignKey("DefaultLocationID");
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.NotificationModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReportModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "Executor")
                        .WithMany()
                        .HasForeignKey("ExecutorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReservationInventoryModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.InventoryModel_DB", "Inventory")
                        .WithMany("ReservationInventories")
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Inventory.ReservationModel_DB", "Reservation")
                        .WithMany("ReservationInventories")
                        .HasForeignKey("ReservationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AIMAS.Data.Inventory.ReservationModel_DB", b =>
                {
                    b.HasOne("AIMAS.Data.Inventory.LocationModel_DB", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Identity.UserModel_DB", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.RoleModel_DB")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.UserModel_DB")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.UserModel_DB")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.RoleModel_DB")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AIMAS.Data.Identity.UserModel_DB")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("AIMAS.Data.Identity.UserModel_DB")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
