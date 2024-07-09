using System;
using System.Collections.Generic;
using System.Configuration;
using Incharge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Incharge.Data;

public partial class InchargeContext : IdentityDbContext<User>
{
    public InchargeContext(DbContextOptions<InchargeContext> options) : base(options) { }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the primary key for IdentityUserLogin
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        modelBuilder.Entity<BusinessReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("business_report");


            entity.HasIndex(e => e.Uuid, "BusinessReport_Uuid_UNIQUE").IsUnique();

            entity.Property(e => e.Uuid)
               .HasDefaultValueSql("(uuid())")
               .IsFixedLength();


            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        modelBuilder.Entity<Client>(entity => //make id column not null.
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.Uuid, "Client_Uuid_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.FirstName, "FirstName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Client_Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.LastName, "LastName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PaymentRecordId, "PaymentRecordId_idx");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("(uuid())")
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.LastName).HasMaxLength(45);
            entity.Property(e => e.Status).HasMaxLength(45);

            entity.HasOne(d => d.PaymentRecord).WithOne(p => p.Clients)
                .HasForeignKey<Client>(d => d.PaymentRecordId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PaymentRecordId");

            entity.HasMany(d => d.Employees).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientEmployee",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("Employeeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_employee_ibfk_2"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_employee_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Clientid", "Employeeid").HasName("PRIMARY");
                        j.ToTable("client_employee");
                        j.HasIndex(new[] { "Employeeid" }, "employeeid");
                        j.IndexerProperty<int>("Clientid")
                            .HasColumnName("clientid");
                        j.IndexerProperty<int>("Employeeid")
                            .HasColumnName("employeeid");
                    });

            entity.HasMany(d => d.Gymclasses).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientGymclass",
                    r => r.HasOne<Gymclass>().WithMany()
                        .HasForeignKey("Gymclassid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_gymclasses_ibfk_2"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("Clientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_gymclasses_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Clientid", "Gymclassid").HasName("PRIMARY");
                        j.ToTable("client_gymclasses");
                        j.HasIndex(new[] { "Gymclassid" }, "gymclassid");
                        j.IndexerProperty<int>("Clientid")
                            .HasColumnName("clientid");
                        j.IndexerProperty<int>("Gymclassid").HasColumnName("gymclassid");
                    });
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("discounts");

            entity.Property(e => e.DiscountValue)
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

            entity.HasIndex(e => e.Id, "Employee_Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Uuid, "Employee_Uuid_UNIQUE").IsUnique();

            entity.HasIndex(e => e.RoleId, "RoleId_idx");

            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasDefaultValueSql("(uuid())")
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
            entity.Property(e => e.Status)
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

            entity.HasIndex(e => e.Uuid, "Expenses_Uuid_UNIQUE").IsUnique();

            entity.Property(e => e.Uuid)
                  .HasDefaultValueSql("(uuid())")
                  .IsFixedLength();

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
            entity.Property(e => e.Status).HasDefaultValueSql("Available");
        });

        modelBuilder.Entity<Paymentrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paymentrecord");


            entity.HasIndex(e => e.Uuid, "PaymentRecord_Uuid_UNIQUE").IsUnique();

            //entity.Property(e => e.Uuid)
            //      .HasDefaultValueSql("(uuid())")
            //      .IsFixedLength();

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
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_product_ibfk_2"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("client_product_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Productid", "Clientid").HasName("PRIMARY");
                        j.ToTable("client_product");
                        j.HasIndex(new[] { "Clientid" }, "clientid");
                        j.IndexerProperty<int>("Productid").HasColumnName("productid");
                        j.IndexerProperty<int>("Clientid")
                            .HasMaxLength(255)
                            .IsFixedLength()
                            .HasColumnName("clientid");
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


            entity.HasIndex(e => e.Uuid, "Sales_Uuid_UNIQUE").IsUnique();


            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.Uuid)
               .HasDefaultValueSql("(uuid())")
               .IsFixedLength();

            entity.HasOne(d => d.BusinessReport).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BusinessReportId)
                .HasConstraintName("BusinessReportId");

            entity.HasOne(d => d.Client).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Client_idfk");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Employe_idfk");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ProductId");

            entity.HasMany(d => d.Discounts).WithMany(p => p.Sales)
            .UsingEntity<Dictionary<string, object>>(
            "SaleDiscount",
            r => r.HasOne<Discount>().WithMany()
                .HasForeignKey("Discountid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sale_discount_ibfk_2"),
            l => l.HasOne<Sale>().WithMany()
                .HasForeignKey("Saleid")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sale_discount_ibfk_1"),
            j =>
            {
                j.HasKey("Saleid", "Discountid").HasName("PRIMARY");
                j.ToTable("sale_discount");
                j.HasIndex(new[] { "Discountid" }, "discountid");
                j.IndexerProperty<int>("Saleid").HasColumnName("saleid");
                j.IndexerProperty<int>("Discountid").HasColumnName("discountid");
            });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
