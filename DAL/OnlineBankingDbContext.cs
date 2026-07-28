using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication.DAL;

public partial class OnlineBankingDbContext : DbContext
{
    public OnlineBankingDbContext()
    {
    }

    public OnlineBankingDbContext(DbContextOptions<OnlineBankingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<BillPayment> BillPayments { get; set; }

    public virtual DbSet<ChequeBookRequest> ChequeBookRequests { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<UtilityBill> UtilityBills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DHRUVI\\SQLEXPRESS;Database=OnlineBankingDB1;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Admin__B40CC6CDB9B98064");

            entity.ToTable("Admin");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinimumBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ProductType).HasMaxLength(30);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F2398305CFE6C");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__BankAcco__349DA5A61B68F450");

            entity.HasIndex(e => e.AccountNumber, "UQ__BankAcco__BE2ACD6FC06F0C7C").IsUnique();

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.OpenedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Customer).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_Customers");

            entity.HasOne(d => d.Product).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_Product");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.BeneficiaryId).HasName("PK__Benefici__3FBA95F51CEB5313");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BeneficiaryName).HasMaxLength(100);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.NickName).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Beneficiary_Customer");
        });

        modelBuilder.Entity<BillPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__BillPaym__9B556A38FDDEB908");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Paid");

            entity.HasOne(d => d.Account).WithMany(p => p.BillPayments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillPayments_Account");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillPayments)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BillPayments_Bill");

            entity.HasOne(d => d.Transaction).WithMany(p => p.BillPayments)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_BillPayments_Transaction");
        });

        modelBuilder.Entity<ChequeBookRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__ChequeBo__33A8517A52043CD3");

            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovedBy).HasMaxLength(450);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Account).WithMany(p => p.ChequeBookRequests)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChequeRequest_Account");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8F7731090");

            entity.HasIndex(e => e.Pannumber, "UQ__Customer__04D051B40A2AECB4").IsUnique();

            entity.HasIndex(e => e.AadhaarNumber, "UQ__Customer__72CF795925F47F05").IsUnique();

            entity.Property(e => e.AadhaarNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Pannumber)
                .HasMaxLength(20)
                .HasColumnName("PANNumber");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Pincode).HasMaxLength(10);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B72374B02");

            entity.HasIndex(e => e.TransactionReference, "UQ__Transact__6783D2A1814146F2").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Success");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.ReceiverAccount).WithMany(p => p.TransactionReceiverAccounts)
                .HasForeignKey(d => d.ReceiverAccountId)
                .HasConstraintName("FK_Transaction_Receiver");

            entity.HasOne(d => d.SenderAccount).WithMany(p => p.TransactionSenderAccounts)
                .HasForeignKey(d => d.SenderAccountId)
                .HasConstraintName("FK_Transaction_Sender");
        });

        modelBuilder.Entity<UtilityBill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__UtilityB__11F2FC6A312DC860");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BillType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProviderName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
