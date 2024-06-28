﻿// <auto-generated />
using System;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Incharge.Migrations
{
    [DbContext(typeof(InchargeContext))]
    [Migration("20240628065704_locationStatusString")]
    partial class locationStatusString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ClientEmployee", b =>
                {
                    b.Property<int>("Clientid")
                        .HasColumnType("int")
                        .HasColumnName("clientid");

                    b.Property<int>("Employeeid")
                        .HasColumnType("int")
                        .HasColumnName("employeeid");

                    b.HasKey("Clientid", "Employeeid")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Employeeid" }, "employeeid");

                    b.ToTable("client_employee", (string)null);
                });

            modelBuilder.Entity("ClientGymclass", b =>
                {
                    b.Property<int>("Clientid")
                        .HasColumnType("int")
                        .HasColumnName("clientid");

                    b.Property<int>("Gymclassid")
                        .HasColumnType("int")
                        .HasColumnName("gymclassid");

                    b.HasKey("Clientid", "Gymclassid")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Gymclassid" }, "gymclassid");

                    b.ToTable("client_gymclasses", (string)null);
                });

            modelBuilder.Entity("ClientProduct", b =>
                {
                    b.Property<int>("Productid")
                        .HasColumnType("int")
                        .HasColumnName("productid");

                    b.Property<int>("Clientid")
                        .HasMaxLength(255)
                        .HasColumnType("int")
                        .HasColumnName("clientid")
                        .IsFixedLength();

                    b.HasKey("Productid", "Clientid")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Clientid" }, "clientid");

                    b.ToTable("client_product", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.BusinessReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Cost")
                        .HasColumnType("double");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<double?>("Revenue")
                        .HasColumnType("double");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Uuid" }, "BusinessReport_Uuid_UNIQUE")
                        .IsUnique();

                    b.ToTable("business_report", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int?>("PaymentRecordId")
                        .HasColumnType("int");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(255)")
                        .HasDefaultValueSql("(uuid())")
                        .IsFixedLength();

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Id" }, "Client_Id_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "Uuid" }, "Client_Uuid_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "Email_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "FirstName" }, "FirstName_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "LastName" }, "LastName_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "PaymentRecordId" }, "PaymentRecordId_idx");

                    b.ToTable("clients", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Discount1")
                        .HasPrecision(10)
                        .HasColumnType("decimal(10)")
                        .HasColumnName("Discount");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Recurance")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("discounts", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<double?>("TotalSalary")
                        .HasColumnType("double");

                    b.Property<string>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("char(255)")
                        .HasDefaultValueSql("(uuid())")
                        .IsFixedLength();

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Id" }, "Employee_Id_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "Uuid" }, "Employee_Uuid_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "RoleId" }, "RoleId_idx");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.EmployeeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Salary")
                        .HasColumnType("double");

                    b.Property<string>("Type")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Id" }, "id_UNIQUE")
                        .IsUnique();

                    b.ToTable("employee_type", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("GymClassId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("MaintanceDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasDefaultValueSql("'Available'");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "GymClassId" }, "GymClassId_idx");

                    b.ToTable("equipment", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BusinessReportId")
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("double");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Uuid" }, "Expenses_Uuid_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "BusinessReportId" }, "business_reportId_idx");

                    b.ToTable("expenses", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Gymclass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "EmployeeId" }, "EmployeeId_idx");

                    b.HasIndex(new[] { "LocationId" }, "LocationId_idx");

                    b.ToTable("gymclass", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValueSql("'Available'");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("location", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Paymentrecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.Property<bool?>("Paymentstatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("paymentstatus")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Uuid" }, "PaymentRecord_Uuid_UNIQUE")
                        .IsUnique();

                    b.ToTable("paymentrecord", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("ProductTypeId")
                        .HasColumnType("int");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("double");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductTypeId" }, "ProductTypeId_idx");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Producttype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("Recurance")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("producttype", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BusinessReportId")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "BusinessReportId" }, "BusinessReportId_idx");

                    b.HasIndex(new[] { "ClientId" }, "ClientSales_idfk");

                    b.HasIndex(new[] { "EmployeeId" }, "EmployeeSales_idfk1");

                    b.HasIndex(new[] { "ProductId" }, "ProductId_idx");

                    b.HasIndex(new[] { "Uuid" }, "Sales_Uuid_UNIQUE")
                        .IsUnique();

                    b.ToTable("sales", (string)null);
                });

            modelBuilder.Entity("Incharge.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProductDiscount", b =>
                {
                    b.Property<int>("Productid")
                        .HasColumnType("int")
                        .HasColumnName("productid");

                    b.Property<int>("Discountid")
                        .HasColumnType("int")
                        .HasColumnName("discountid");

                    b.HasKey("Productid", "Discountid")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Discountid" }, "discountid");

                    b.ToTable("product_discount", (string)null);
                });

            modelBuilder.Entity("ClientEmployee", b =>
                {
                    b.HasOne("Incharge.Models.Client", null)
                        .WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_employee_ibfk_1");

                    b.HasOne("Incharge.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("Employeeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_employee_ibfk_2");
                });

            modelBuilder.Entity("ClientGymclass", b =>
                {
                    b.HasOne("Incharge.Models.Client", null)
                        .WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_gymclasses_ibfk_1");

                    b.HasOne("Incharge.Models.Gymclass", null)
                        .WithMany()
                        .HasForeignKey("Gymclassid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_gymclasses_ibfk_2");
                });

            modelBuilder.Entity("ClientProduct", b =>
                {
                    b.HasOne("Incharge.Models.Client", null)
                        .WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_product_ibfk_2");

                    b.HasOne("Incharge.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("client_product_ibfk_1");
                });

            modelBuilder.Entity("Incharge.Models.Client", b =>
                {
                    b.HasOne("Incharge.Models.Paymentrecord", "PaymentRecord")
                        .WithMany("Clients")
                        .HasForeignKey("PaymentRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("PaymentRecordId");

                    b.Navigation("PaymentRecord");
                });

            modelBuilder.Entity("Incharge.Models.Employee", b =>
                {
                    b.HasOne("Incharge.Models.EmployeeType", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Incharge.Models.Equipment", b =>
                {
                    b.HasOne("Incharge.Models.Gymclass", "GymClass")
                        .WithMany("Equipment")
                        .HasForeignKey("GymClassId")
                        .HasConstraintName("GymClassId");

                    b.Navigation("GymClass");
                });

            modelBuilder.Entity("Incharge.Models.Expense", b =>
                {
                    b.HasOne("Incharge.Models.BusinessReport", "BusinessReport")
                        .WithMany("Expenses")
                        .HasForeignKey("BusinessReportId")
                        .HasConstraintName("business_reportId");

                    b.Navigation("BusinessReport");
                });

            modelBuilder.Entity("Incharge.Models.Gymclass", b =>
                {
                    b.HasOne("Incharge.Models.Employee", "Employee")
                        .WithMany("Gymclasses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("EmployeeId");

                    b.HasOne("Incharge.Models.Location", "Location")
                        .WithMany("Gymclasses")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LocationId");

                    b.Navigation("Employee");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Incharge.Models.Product", b =>
                {
                    b.HasOne("Incharge.Models.Producttype", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("ProductTypeId");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("Incharge.Models.Sale", b =>
                {
                    b.HasOne("Incharge.Models.BusinessReport", "BusinessReport")
                        .WithMany("Sales")
                        .HasForeignKey("BusinessReportId")
                        .HasConstraintName("BusinessReportId");

                    b.HasOne("Incharge.Models.Client", "Client")
                        .WithMany("Sales")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Client_idfk");

                    b.HasOne("Incharge.Models.Employee", "Employee")
                        .WithMany("Sales")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Employe_idfk");

                    b.HasOne("Incharge.Models.Product", "Product")
                        .WithMany("Sales")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("ProductId");

                    b.Navigation("BusinessReport");

                    b.Navigation("Client");

                    b.Navigation("Employee");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Incharge.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Incharge.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incharge.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Incharge.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductDiscount", b =>
                {
                    b.HasOne("Incharge.Models.Discount", null)
                        .WithMany()
                        .HasForeignKey("Discountid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("product_discount_ibfk_2");

                    b.HasOne("Incharge.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("product_discount_ibfk_1");
                });

            modelBuilder.Entity("Incharge.Models.BusinessReport", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Incharge.Models.Client", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Incharge.Models.Employee", b =>
                {
                    b.Navigation("Gymclasses");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Incharge.Models.EmployeeType", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Incharge.Models.Gymclass", b =>
                {
                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("Incharge.Models.Location", b =>
                {
                    b.Navigation("Gymclasses");
                });

            modelBuilder.Entity("Incharge.Models.Paymentrecord", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("Incharge.Models.Product", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Incharge.Models.Producttype", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
