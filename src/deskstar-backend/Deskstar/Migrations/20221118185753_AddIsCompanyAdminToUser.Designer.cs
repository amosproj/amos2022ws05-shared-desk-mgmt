﻿// <auto-generated />
using System;
using Deskstar.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Deskstar.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221118185753_AddIsCompanyAdminToUser")]
    partial class AddIsCompanyAdminToUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Deskstar.Entities.Booking", b =>
                {
                    b.Property<Guid>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("BookingID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("DeskId")
                        .HasColumnType("uuid")
                        .HasColumnName("DeskID");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserID");

                    b.HasKey("BookingId");

                    b.HasIndex("DeskId");

                    b.HasIndex("UserId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Building", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("BuildingID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("BuildingName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("CompanyID");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("BuildingId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Building", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Company", b =>
                {
                    b.Property<Guid>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("CompanyID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<bool?>("Logo")
                        .HasColumnType("boolean");

                    b.HasKey("CompanyId");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Desk", b =>
                {
                    b.Property<Guid>("DeskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("DeskID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("DeskName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<Guid>("DeskTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("DeskTypeID");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoomID");

                    b.HasKey("DeskId");

                    b.HasIndex("DeskTypeId");

                    b.HasIndex("RoomId");

                    b.ToTable("Desk", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.DeskType", b =>
                {
                    b.Property<Guid>("DeskTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("DeskTypeID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("CompanyID");

                    b.Property<string>("DeskTypeName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("DeskTypeId");

                    b.HasIndex("CompanyId");

                    b.ToTable("DeskType", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Floor", b =>
                {
                    b.Property<Guid>("FloorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("FloorID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid")
                        .HasColumnName("BuildingID");

                    b.Property<string>("FloorName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("FloorId");

                    b.HasIndex("BuildingId");

                    b.ToTable("Floor", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("RoleID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("CompanyID");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("RoleId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Room", b =>
                {
                    b.Property<Guid>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("RoomID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("FloorId")
                        .HasColumnType("uuid")
                        .HasColumnName("FloorID");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("RoomId");

                    b.HasIndex("FloorId");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("UserID")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("CompanyID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<bool>("IsApproved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("false");

                    b.Property<bool>("IsCompanyAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValueSql("false");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<string>("MailAddress")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.HasKey("UserId");

                    b.HasIndex("CompanyId");

                    b.HasIndex(new[] { "MailAddress" }, "User_Mail")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoleID");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserID");

                    b.HasKey("RoleId", "UserId")
                        .HasName("UserRole_pk");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Deskstar.Entities.Booking", b =>
                {
                    b.HasOne("Deskstar.Entities.Desk", "Desk")
                        .WithMany("Bookings")
                        .HasForeignKey("DeskId")
                        .IsRequired()
                        .HasConstraintName("Booking_Desk_null_fk");

                    b.HasOne("Deskstar.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("Booking_User_null_fk");

                    b.Navigation("Desk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Deskstar.Entities.Building", b =>
                {
                    b.HasOne("Deskstar.Entities.Company", "Company")
                        .WithMany("Buildings")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("foreign_key_name");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Deskstar.Entities.Desk", b =>
                {
                    b.HasOne("Deskstar.Entities.DeskType", "DeskType")
                        .WithMany("Desks")
                        .HasForeignKey("DeskTypeId")
                        .IsRequired()
                        .HasConstraintName("Desk_DeskType_null_fk");

                    b.HasOne("Deskstar.Entities.Room", "Room")
                        .WithMany("Desks")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("Desk_Room_null_fk");

                    b.Navigation("DeskType");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Deskstar.Entities.DeskType", b =>
                {
                    b.HasOne("Deskstar.Entities.Company", "Company")
                        .WithMany("DeskTypes")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("DeskType_Company_null_fk");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Deskstar.Entities.Floor", b =>
                {
                    b.HasOne("Deskstar.Entities.Building", "Building")
                        .WithMany("Floors")
                        .HasForeignKey("BuildingId")
                        .IsRequired()
                        .HasConstraintName("Floor_Building_null_fk");

                    b.Navigation("Building");
                });

            modelBuilder.Entity("Deskstar.Entities.Role", b =>
                {
                    b.HasOne("Deskstar.Entities.Company", "Company")
                        .WithMany("Roles")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("Role_Company_null_fk");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Deskstar.Entities.Room", b =>
                {
                    b.HasOne("Deskstar.Entities.Floor", "Floor")
                        .WithMany("Rooms")
                        .HasForeignKey("FloorId")
                        .IsRequired()
                        .HasConstraintName("Room_Floor_null_fk");

                    b.Navigation("Floor");
                });

            modelBuilder.Entity("Deskstar.Entities.User", b =>
                {
                    b.HasOne("Deskstar.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("CompanyID_fk");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("Deskstar.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("UserRole_Role_null_fk");

                    b.HasOne("Deskstar.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("UserRole_User_null_fk");
                });

            modelBuilder.Entity("Deskstar.Entities.Building", b =>
                {
                    b.Navigation("Floors");
                });

            modelBuilder.Entity("Deskstar.Entities.Company", b =>
                {
                    b.Navigation("Buildings");

                    b.Navigation("DeskTypes");

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Deskstar.Entities.Desk", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Deskstar.Entities.DeskType", b =>
                {
                    b.Navigation("Desks");
                });

            modelBuilder.Entity("Deskstar.Entities.Floor", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Deskstar.Entities.Room", b =>
                {
                    b.Navigation("Desks");
                });

            modelBuilder.Entity("Deskstar.Entities.User", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}