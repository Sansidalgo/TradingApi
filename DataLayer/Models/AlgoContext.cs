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

    public virtual DbSet<TblDelegate> TblDelegates { get; set; }

    public virtual DbSet<TblEnvironment> TblEnvironments { get; set; }

    public virtual DbSet<TblInstrument> TblInstruments { get; set; }

    public virtual DbSet<TblOffer> TblOffers { get; set; }

    public virtual DbSet<TblOptionsDatum> TblOptionsData { get; set; }

    public virtual DbSet<TblOptionsSetting> TblOptionsSettings { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderSetting> TblOrderSettings { get; set; }

    public virtual DbSet<TblOrderSide> TblOrderSides { get; set; }

    public virtual DbSet<TblOrderSource> TblOrderSources { get; set; }

    public virtual DbSet<TblPayment> TblPayments { get; set; }

    public virtual DbSet<TblPaymentStatus> TblPaymentStatuses { get; set; }

    public virtual DbSet<TblPlan> TblPlans { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePlan> TblRolePlans { get; set; }

    public virtual DbSet<TblSegment> TblSegments { get; set; }

    public virtual DbSet<TblShoonyaCredential> TblShoonyaCredentials { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<TblStatusType> TblStatusTypes { get; set; }

    public virtual DbSet<TblStrategy> TblStrategies { get; set; }

    public virtual DbSet<TblSubscriptionStatus> TblSubscriptionStatuses { get; set; }

    public virtual DbSet<TblTraderDetail> TblTraderDetails { get; set; }

    public virtual DbSet<TblTransactionsHistory> TblTransactionsHistories { get; set; }

    public virtual DbSet<TblUserOffer> TblUserOffers { get; set; }

    public virtual DbSet<TblUserPlan> TblUserPlans { get; set; }

    public virtual DbSet<TblUserSubscription> TblUserSubscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=P3NWPLSK12SQL-v12.shr.prod.phx3.secureserver.net;Database=algo;User ID=algo;Password=Siddu_1990@;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("algo");

        modelBuilder.Entity<TblBroker>(entity =>
        {
            entity.ToTable("tblBrokers");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblDelegate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblDeleg__3214EC27ACC46C0D");

            entity.ToTable("tblDelegate");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblDelegateCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__tblDelega__Creat__324172E1");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TblDelegateUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__tblDelega__Updat__3335971A");
        });

        modelBuilder.Entity<TblEnvironment>(entity =>
        {
            entity.ToTable("tblEnvironments");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblInstrument>(entity =>
        {
            entity.ToTable("tblInstruments");

            entity.HasIndex(e => e.Name, "UC_tblInstruments_instrument").IsUnique();

            entity.Property(e => e.Exchange)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblOffer__3214EC07AFED5CEF");

            entity.ToTable("tblOffer");

            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblOptionsDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblOptio__3214EC27B136E96D");

            entity.ToTable("tblOptionsData");

            entity.HasIndex(e => new { e.PcrOi, e.PcrOichange, e.PutOi, e.CallOi, e.PutOichange, e.CallOichange, e.Pevwap, e.Cevwap }, "UQ_OptionsData_Columns").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CallOi).HasColumnName("CallOI");
            entity.Property(e => e.CallOichange).HasColumnName("CallOIChange");
            entity.Property(e => e.Cevwap).HasColumnName("CEVWAP");
            entity.Property(e => e.EntryDateTime).HasColumnType("datetime");
            entity.Property(e => e.PcrOi).HasColumnName("PcrOI");
            entity.Property(e => e.PcrOichange).HasColumnName("PcrOIChange");
            entity.Property(e => e.Pevwap).HasColumnName("PEVWAP");
            entity.Property(e => e.PutOi).HasColumnName("PutOI");
            entity.Property(e => e.PutOichange).HasColumnName("PutOIChange");

            entity.HasOne(d => d.Instrument).WithMany(p => p.TblOptionsData)
                .HasForeignKey(d => d.InstrumentId)
                .HasConstraintName("FK_tblOptionsData_tblInstruments");
        });

        modelBuilder.Entity<TblOptionsSetting>(entity =>
        {
            entity.ToTable("tblOptionsSettings");

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UQ_tblOptionsSettings_name_traderId").IsUnique();

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.EndTime)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Exchange)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PlayCapital).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.StartTime)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDt).HasColumnType("datetime");

            entity.HasOne(d => d.Instrument).WithMany(p => p.TblOptionsSettings)
                .HasForeignKey(d => d.InstrumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOptionsSettings_tblInstruments");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.ToTable("tblOrders");

            entity.Property(e => e.Asset)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.BuyAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.IndexPriceAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.SellAt).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UpdatedDt).HasColumnType("datetime");

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

            entity.HasOne(d => d.Status).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrders_tblStatusTypes");

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
                .IsUnicode(false)
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblEnvironments");

            entity.HasOne(d => d.OptionsSettings).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.OptionsSettingsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblOptionsSettings");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblStrategies");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblOrderSettings)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblOrderSettings_tblTraderDetails");
        });

        modelBuilder.Entity<TblOrderSide>(entity =>
        {
            entity.ToTable("tblOrderSides");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblOrderSource>(entity =>
        {
            entity.ToTable("tblOrderSources");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblPayme__3214EC07E7C9815A");

            entity.ToTable("tblPayments");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDt).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(300);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Offer).WithMany(p => p.TblPayments)
                .HasForeignKey(d => d.OfferId)
                .HasConstraintName("FK_Payment_Offer");

            entity.HasOne(d => d.Status).WithMany(p => p.TblPayments)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__tblPaymen__Statu__0C1BC9F9");

            entity.HasOne(d => d.Subscription).WithMany(p => p.TblPayments)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_Payment_UserSubscription");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblPayments)
                .HasForeignKey(d => d.TraderId)
                .HasConstraintName("FK_Payment_Trader");
        });

        modelBuilder.Entity<TblPaymentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblPayme__3214EC07D620698E");

            entity.ToTable("tblPaymentStatus");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<TblPlan>(entity =>
        {
            entity.ToTable("tblPlans");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.ToTable("tblRoles");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("name");
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

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblShoonyaCredential>(entity =>
        {
            entity.ToTable("tblShoonyaCredentials");

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UC_Name_TraderID").IsUnique();

            entity.HasIndex(e => e.Name, "UQ_tblShoonyaCredentials_Name").IsUnique();

            entity.HasIndex(e => new { e.Name, e.TraderId }, "UQ_tblShoonyaCredentials_name_traderId").IsUnique();

            entity.Property(e => e.ApiKey)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.AuthSecreteKey)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Created_Dt");
            entity.Property(e => e.Imei)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMEI");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Uid)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("UID");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.Vc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VC");
        });

        modelBuilder.Entity<TblStatus>(entity =>
        {
            entity.ToTable("tblStatus");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.TblStatuses)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblStatus_tblOrders");

            entity.HasOne(d => d.StatusType).WithMany(p => p.TblStatuses)
                .HasForeignKey(d => d.StatusTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblStatus_tblStatusTypes1");
        });

        modelBuilder.Entity<TblStatusType>(entity =>
        {
            entity.ToTable("tblStatusTypes");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("name");
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
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblStrategies)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Strategy_TraderDetails");
        });

        modelBuilder.Entity<TblSubscriptionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblSubsc__3214EC07E77FAD22");

            entity.ToTable("tblSubscriptionStatus");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTraderDetail>(entity =>
        {
            entity.ToTable("tblTraderDetails");

            entity.HasIndex(e => e.EmailId, "UC_tblTraderDetails_EmailId").IsUnique();

            entity.HasIndex(e => e.PhoneNo, "UC_tblTraderDetails_PhoneNo").IsUnique();

            entity.HasIndex(e => new { e.PhoneNo, e.EmailId }, "UC_tblTraderDetails_PhoneNo_EmailId").IsUnique();

            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

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
                .IsUnicode(false);
            entity.Property(e => e.TraderId).HasColumnName("TraderID");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Trader).WithMany(p => p.TblTransactionsHistories)
                .HasForeignKey(d => d.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblTransactionsHistory_tblTraderDetails");
        });

        modelBuilder.Entity<TblUserOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblUserO__3214EC0700804B42");

            entity.ToTable("tblUserOffer");

            entity.Property(e => e.AppliedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Offer).WithMany(p => p.TblUserOffers)
                .HasForeignKey(d => d.OfferId)
                .HasConstraintName("FK_UserOffer_Offer");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblUserOffers)
                .HasForeignKey(d => d.TraderId)
                .HasConstraintName("FK_UserOffer_User");
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

        modelBuilder.Entity<TblUserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblUserS__3214EC079F43CBEC");

            entity.ToTable("tblUserSubscriptions");

            entity.Property(e => e.EndDt).HasColumnType("datetime");
            entity.Property(e => e.StartDt).HasColumnType("datetime");

            entity.HasOne(d => d.Plan).WithMany(p => p.TblUserSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK__tblUserSu__PlanI__73501C2F");

            entity.HasOne(d => d.SubscriptionStatus).WithMany(p => p.TblUserSubscriptions)
                .HasForeignKey(d => d.SubscriptionStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblUserSu__Subsc__74444068");

            entity.HasOne(d => d.Trader).WithMany(p => p.TblUserSubscriptions)
                .HasForeignKey(d => d.TraderId)
                .HasConstraintName("FK__tblUserSu__Trade__725BF7F6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
