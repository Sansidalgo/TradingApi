using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models;

public partial class AlgoContext : DbContext
{
    public AlgoContext()
    {
    }

    public AlgoContext(DbContextOptions<AlgoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBroker> TblBrokers { get; set; }

    public virtual DbSet<TblEnvironment> TblEnvironments { get; set; }

    public virtual DbSet<TblInstrument> TblInstruments { get; set; }

    public virtual DbSet<TblOptionsSetting> TblOptionsSettings { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderSetting> TblOrderSettings { get; set; }

    public virtual DbSet<TblOrderSide> TblOrderSides { get; set; }

    public virtual DbSet<TblOrderSource> TblOrderSources { get; set; }

    public virtual DbSet<TblPlan> TblPlans { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePlan> TblRolePlans { get; set; }

    public virtual DbSet<TblSegment> TblSegments { get; set; }

    public virtual DbSet<TblShoonyaCredential> TblShoonyaCredentials { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<TblStatusType> TblStatusTypes { get; set; }

    public virtual DbSet<TblStrategy> TblStrategies { get; set; }

    public virtual DbSet<TblSubscription> TblSubscriptions { get; set; }

    public virtual DbSet<TblTraderDetail> TblTraderDetails { get; set; }

    public virtual DbSet<TblTransactionsHistory> TblTransactionsHistories { get; set; }

    public virtual DbSet<TblUserPlan> TblUserPlans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=P3NWPLSK12SQL-v12.shr.prod.phx3.secureserver.net;Database=algo;User ID=algo;Password=Siddu_1990@;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("algo");

        modelBuilder.Entity<TblBroker>(entity =>
        {
            entity.ToTable("tblBrokers");

            entity.Property(e => e.Broker)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblEnvironment>(entity =>
        {
            entity.ToTable("tblEnvironments");

            entity.Property(e => e.Environment)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblInstrument>(entity =>
        {
            entity.ToTable("tblInstruments");

            entity.HasIndex(e => e.Instrument, "UC_tblInstruments_instrument").IsUnique();

            entity.Property(e => e.Instrument)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblOptionsSetting>(entity =>
        {
            entity.ToTable("tblOptionsSettings");

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UQ_tblOptionsSettings_name_traderId").IsUnique();

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Exchange)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDay)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PlayCapital).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.PlayQuantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TraderId).HasColumnName("TraderID");
            entity.Property(e => e.UpdatedDt).HasColumnType("datetime");

            entity.HasOne(d => d.Instrument).WithMany(p => p.TblOptionsSettings)
                .HasForeignKey(d => d.InstrumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOptionsSettings_tblInstruments");

            entity.HasOne(d => d.Strategy).WithMany(p => p.TblOptionsSettings)
                .HasForeignKey(d => d.StrategyId)
                .HasConstraintName("FK_tblOptionsSettings_tblStrategies");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblOptionsSettings)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOptionsSettings_tblTraderDetails1");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.ToTable("tblOrders");

            entity.Property(e => e.Asset)
                .HasMaxLength(300)
                .IsFixedLength();
            entity.Property(e => e.BuyAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.IndexPriceAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.SellAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UpdatedDt).HasColumnType("datetime");

            entity.HasOne(d => d.Environment).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.EnvironmentId)
                .HasConstraintName("FK_tblOrders_tblEnvironments");

            entity.HasOne(d => d.OrderSettings).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.OrderSettingsId)
                .HasConstraintName("FK_tblOrders_tblOrderSettings");

            entity.HasOne(d => d.OrderSide).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.OrderSideId)
                .HasConstraintName("FK_tblOrders_tblOrderSides");

            entity.HasOne(d => d.OrderSource).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.OrderSourceId)
                .HasConstraintName("FK_tblOrders_tblOrderSources");

            entity.HasOne(d => d.Segment).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.SegmentId)
                .HasConstraintName("FK_tblOrders_tblSegments");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrders_tblTraderDetails");
        });

        modelBuilder.Entity<TblOrderSetting>(entity =>
        {
            entity.ToTable("tblOrderSettings");

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UQ_tblOrderSettings_name_traderId").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsFixedLength()
                .HasColumnName("name");

            entity.HasOne(d => d.BrokerCredentials).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.BrokerCredentialsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblShoonyaCredentials");

            entity.HasOne(d => d.Broker).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.BrokerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblBrokers");

            entity.HasOne(d => d.Environment).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.EnvironmentId)
                .HasConstraintName("FK_tblOrderSettings_tblEnvironments");

            entity.HasOne(d => d.Instrument).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.InstrumentId)
                .HasConstraintName("FK_tblOrderSettings_tblInstruments");

            entity.HasOne(d => d.OptionsSettings).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.OptionsSettingsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblOptionsSettings1");

            entity.HasOne(d => d.OrderSide).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.OrderSideId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblOrderSides");

            entity.HasOne(d => d.OrderSource).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.OrderSourceId)
                .HasConstraintName("FK_tblOrderSettings_tblOrderSources");

            entity.HasOne(d => d.Segment).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.SegmentId)
                .HasConstraintName("FK_tblOrderSettings_tblSegments");

            entity.HasOne(d => d.Strategy).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.StrategyId)
                .HasConstraintName("FK_tblOrderSettings_tblStrategies");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblTraderDetails");
        });

        modelBuilder.Entity<TblOrderSide>(entity =>
        {
            entity.ToTable("tblOrderSides");

            entity.Property(e => e.Side)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblOrderSource>(entity =>
        {
            entity.ToTable("tblOrderSources");

            entity.Property(e => e.Source)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblPlan>(entity =>
        {
            entity.ToTable("tblPlans");

            entity.Property(e => e.Plan)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.ToTable("tblRoles");

            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblRolePlan>(entity =>
        {
            entity.ToTable("tblRolePlans");

            entity.HasOne(d => d.Plan).WithMany(p => p.TblRolePlans)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_tblRolePlans_tblPlans");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePlans)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_tblRolePlans_tblRoles");
        });

        modelBuilder.Entity<TblSegment>(entity =>
        {
            entity.ToTable("tblSegments");

            entity.Property(e => e.Segment)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblShoonyaCredential>(entity =>
        {
            entity.ToTable("tblShoonyaCredentials");

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UC_Name_TraderID").IsUnique();

            entity.HasIndex(e => e.Name, "UQ_tblShoonyaCredentials_Name").IsUnique();

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UQ_tblShoonyaCredentials_name_traderId").IsUnique();

            entity.Property(e => e.ApiKey)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.AuthSecreteKey)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.Imei)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("IMEI");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Uid)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("UID");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.Vc)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("VC");
        });

        modelBuilder.Entity<TblStatus>(entity =>
        {
            entity.ToTable("tblStatus");

            entity.HasOne(d => d.StatusType).WithMany(p => p.TblStatuses)
                .HasForeignKey(d => d.StatusTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblStatus_tblStatusTypes");
        });

        modelBuilder.Entity<TblStatusType>(entity =>
        {
            entity.ToTable("tblStatusTypes");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblStrategy>(entity =>
        {
            entity.ToTable("tblStrategies");

            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Strategy)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblStrategies)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Strategy_TraderDetails");
        });

        modelBuilder.Entity<TblSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblSubscriptions_1");

            entity.ToTable("tblSubscriptions");

            entity.Property(e => e.EndDt)
                .HasColumnType("datetime")
                .HasColumnName("End_Dt");
            entity.Property(e => e.StartDt)
                .HasColumnType("datetime")
                .HasColumnName("Start_Dt");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblSubscriptions)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblSubscriptions_tblTraderDetails1");
        });

        modelBuilder.Entity<TblTraderDetail>(entity =>
        {
            entity.ToTable("tblTraderDetails");

            entity.HasIndex(e => e.EmailId, "UC_tblTraderDetails_EmailId").IsUnique();

            entity.HasIndex(e => e.PhoneNo, "UC_tblTraderDetails_PhoneNo").IsUnique();

            entity.HasIndex(e => new { e.PhoneNo, e.EmailId }, "UC_tblTraderDetails_PhoneNo_EmailId").IsUnique();

            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.TblTraderDetails)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_tblTraderDetails_tblRoles");
        });

        modelBuilder.Entity<TblTransactionsHistory>(entity =>
        {
            entity.ToTable("tblTransactionsHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.TraderId).HasColumnName("TraderID");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Subsription).WithMany(p => p.TblTransactionsHistories)
                .HasForeignKey(d => d.SubsriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblTransactionsHistory_tblSubscriptions");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblTransactionsHistories)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblTransactionsHistory_tblTraderDetails");
        });

        modelBuilder.Entity<TblUserPlan>(entity =>
        {
            entity.ToTable("tblUserPlans");

            entity.HasOne(d => d.Plan).WithMany(p => p.TblUserPlans)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblUserPlans_tblPlans");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblUserPlans_tblTraderDetails");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
