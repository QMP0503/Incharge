﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Models;

public partial class InchargeContext : DbContext
{
    public InchargeContext()
    {
    }

    public InchargeContext(DbContextOptions<InchargeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BusinessReport> BusinessReports { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Gymclass> Gymclasses { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Paymentrecord> Paymentrecords { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=incharge;User=root;Password=Munmeo0503.;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("business_report");

            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.FirstName, "FirstName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            
            entity.HasIndex(e => e.LastName, "LastName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PaymentRecordId, "PaymentRecordId_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(16)
                .HasDefaultValueSql("'uuid_to_bin(uuid())'")
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.LastName).HasMaxLength(45);
            entity.Property(e => e.Status).HasMaxLength(45);

            entity.HasOne(d => d.PaymentRecord).WithMany(p => p.Clients)
                .HasForeignKey(d => d.PaymentRecordId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PaymentRecordId");

            entity.HasMany(d => d.Employees).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientEmployee",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("Employeeid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_employee_ibfk_2"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_employee_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Clientid", "Employeeid").HasName("PRIMARY");
                        j.ToTable("client_employee");
                        j.HasIndex(new[] { "Employeeid" }, "employeeid");
                        j.IndexerProperty<byte[]>("Clientid")
                            .HasMaxLength(16)
                            .IsFixedLength()
                            .HasColumnName("clientid");
                        j.IndexerProperty<byte[]>("Employeeid")
                            .HasMaxLength(16)
                            .IsFixedLength()
                            .HasColumnName("employeeid");
                    });

            entity.HasMany(d => d.Gymclasses).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientGymclass",
                    r => r.HasOne<Gymclass>().WithMany()
                        .HasForeignKey("Gymclassid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_gymclasses_ibfk_2"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_gymclasses_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Clientid", "Gymclassid").HasName("PRIMARY");
                        j.ToTable("client_gymclasses");
                        j.HasIndex(new[] { "Gymclassid" }, "gymclassid");
                        j.IndexerProperty<byte[]>("Clientid")
                            .HasMaxLength(16)
                            .IsFixedLength()
                            .HasColumnName("clientid");
                        j.IndexerProperty<int>("Gymclassid").HasColumnName("gymclassid");
                    });
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("discounts");

            entity.Property(e => e.Discount1)
                .HasPrecision(10)
                .HasColumnName("Discount");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Recurance).HasMaxLength(45);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.RoleId, "RoleId_idx");

            entity.Property(e => e.Id)
                .HasMaxLength(16)
                .HasDefaultValueSql("'uuid_to_bin(uuid())'")
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.LastName).HasMaxLength(45);

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("RoleId");
        });

        modelBuilder.Entity<EmployeeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employee_type");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type).HasMaxLength(45);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("equipment");

            entity.HasIndex(e => e.GymClassId, "GymClassId_idx");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MaintanceDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.Satus)
                .HasMaxLength(45)
                .HasDefaultValueSql("'Available'");

            entity.HasOne(d => d.GymClass).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.GymClassId)
                .HasConstraintName("GymClassId");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("expenses");

            entity.HasIndex(e => e.BusinessReportId, "business_reportId_idx");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);

            entity.HasOne(d => d.BusinessReport).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.BusinessReportId)
                .HasConstraintName("business_reportId");
        });

        modelBuilder.Entity<Gymclass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gymclass");

            entity.HasIndex(e => e.EmployeeId, "EmployeeId_idx");

            entity.HasIndex(e => e.LocationId, "LocationId_idx");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(45);

            entity.HasOne(d => d.Employee).WithMany(p => p.Gymclasses)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("EmployeeId");

            entity.HasOne(d => d.Location).WithMany(p => p.Gymclasses)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("LocationId");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("location");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Paymentrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paymentrecord");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Paymentstatus)
                .HasDefaultValueSql("'0'")
                .HasColumnName("paymentstatus");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.ProductTypeId, "ProductTypeId_idx");

            entity.Property(e => e.Name).HasMaxLength(45);

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("ProductTypeId");

            entity.HasMany(d => d.Clients).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientProduct",
                    r => r.HasOne<Client>().WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_product_ibfk_2"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("client_product_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Productid", "Clientid").HasName("PRIMARY");
                        j.ToTable("client_product");
                        j.HasIndex(new[] { "Clientid" }, "clientid");
                        j.IndexerProperty<int>("Productid").HasColumnName("productid");
                        j.IndexerProperty<byte[]>("Clientid")
                            .HasMaxLength(16)
                            .IsFixedLength()
                            .HasColumnName("clientid");
                    });

            entity.HasMany(d => d.Discounts).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductDiscount",
                    r => r.HasOne<Discount>().WithMany()
                        .HasForeignKey("Discountid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_discount_ibfk_2"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_discount_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Productid", "Discountid").HasName("PRIMARY");
                        j.ToTable("product_discount");
                        j.HasIndex(new[] { "Discountid" }, "discountid");
                        j.IndexerProperty<int>("Productid").HasColumnName("productid");
                        j.IndexerProperty<int>("Discountid").HasColumnName("discountid");
                    });
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producttype");

            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Recurance).HasMaxLength(45);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sales");

            entity.HasIndex(e => e.BusinessReportId, "BusinessReportId_idx");

            entity.HasIndex(e => e.ClientId, "ClientSales_idfk");

            entity.HasIndex(e => e.EmployeeId, "EmployeeSales_idfk1");

            entity.HasIndex(e => e.ProductId, "ProductId_idx");

            entity.Property(e => e.ClientId)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsFixedLength();

            entity.HasOne(d => d.BusinessReport).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BusinessReportId)
                .HasConstraintName("BusinessReportId");

            entity.HasOne(d => d.Client).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Client_idfk");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Employe_idfk");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ProductId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
