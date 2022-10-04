using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.DBModels
{
    public partial class DimTabContext : DbContext
    {
        public DimTabContext()
        {
        }

        public DimTabContext(DbContextOptions<DimTabContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAppInfo> TblAppInfos { get; set; } = null!;
        public virtual DbSet<TblChart> TblCharts { get; set; } = null!;
        public virtual DbSet<TblDataGrid> TblDataGrids { get; set; } = null!;
        public virtual DbSet<TblHoliday> TblHolidays { get; set; } = null!;
        public virtual DbSet<TblPerson> TblPersons { get; set; } = null!;
        public virtual DbSet<TblPersonInWeekend> TblPersonInWeekends { get; set; } = null!;
        public virtual DbSet<TblPlace> TblPlaces { get; set; } = null!;
        public virtual DbSet<TblShiftInWeekend> TblShiftInWeekends { get; set; } = null!;
        public virtual DbSet<TblShop> TblShops { get; set; } = null!;
        public virtual DbSet<TblUnavailable> TblUnavailables { get; set; } = null!;
        public virtual DbSet<TblUserSelectedShop> TblUserSelectedShops { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(PrivateClass.serverString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAppInfo>(entity =>
            {
                entity.HasKey(e => e.AppVersion);

                entity.ToTable("tblAppInfo");

                entity.Property(e => e.AppVersion).ValueGeneratedNever();
            });

            modelBuilder.Entity<TblChart>(entity =>
            {
                entity.HasKey(e => e.ChartId);

                entity.ToTable("tblCharts");

                entity.HasIndex(e => e.ShopId, "IX_tblCharts_ShopID");

                entity.Property(e => e.ChartId).HasColumnName("ChartID");

                entity.Property(e => e.AddWhen).HasColumnType("datetime");

                entity.Property(e => e.AddWho).HasMaxLength(56);

                entity.Property(e => e.ModWhen).HasColumnType("datetime");

                entity.Property(e => e.ModWho).HasMaxLength(56);

                entity.Property(e => e.Mtbf).HasColumnName("MTBF");

                entity.Property(e => e.Mttr).HasColumnName("MTTR");

                entity.Property(e => e.NumberOfWeek).HasDefaultValueSql("((1))");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblCharts)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tblCharts_tblShops");
            });

            modelBuilder.Entity<TblDataGrid>(entity =>
            {
                entity.HasKey(e => e.DataId);

                entity.ToTable("tblDataGrid");

                entity.HasIndex(e => e.ShopId, "IX_tblDataGrid_ShopID");

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.AddWhen).HasColumnType("datetime");

                entity.Property(e => e.AddWho).HasMaxLength(56);

                entity.Property(e => e.Cause).HasMaxLength(255);

                entity.Property(e => e.DateCreateAction).HasColumnType("date");

                entity.Property(e => e.DateEndAction).HasColumnType("date");

                entity.Property(e => e.DatePlaningAction).HasColumnType("date");

                entity.Property(e => e.Info).HasMaxLength(255);

                entity.Property(e => e.ModWhen).HasColumnType("datetime");

                entity.Property(e => e.ModWho).HasMaxLength(56);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.Problem).HasMaxLength(255);

                entity.Property(e => e.ShopId)
                    .HasColumnName("ShopID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Solve).HasMaxLength(255);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblDataGrids)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tblDataGrid_tblShops");
            });

            modelBuilder.Entity<TblHoliday>(entity =>
            {
                entity.HasKey(e => e.HolidayId);

                entity.ToTable("tblHolidays");

                entity.Property(e => e.HolidayId).HasColumnName("HolidayID");

                entity.Property(e => e.DateFrom).HasColumnType("date");

                entity.Property(e => e.DateTo).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(51)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK_tbPersons");

                entity.ToTable("tblPersons");

                entity.HasIndex(e => e.ShopId, "IX_tblPersons_ShopID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.AddWhen).HasColumnType("datetime");

                entity.Property(e => e.AddWho).HasMaxLength(56);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLogin)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FreeDays).HasDefaultValueSql("((26))");

                entity.Property(e => e.Login).HasMaxLength(28);

                entity.Property(e => e.ModWhen).HasColumnType("datetime");

                entity.Property(e => e.ModWho).HasMaxLength(56);

                entity.Property(e => e.Name)
                    .HasMaxLength(28)
                    .HasDefaultValueSql("(N'Admninistrator')");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PermisionId)
                    .HasColumnName("PermisionID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.Property(e => e.Surname)
                    .HasMaxLength(28)
                    .HasDefaultValueSql("(N'Obszaru')");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblPeople)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tbPersons_tblShops1");
            });

            modelBuilder.Entity<TblPersonInWeekend>(entity =>
            {
                entity.HasKey(e => e.IdworkInWeekend)
                    .HasName("PK_WorkInWeekend");

                entity.ToTable("tblPersonInWeekend");

                entity.HasIndex(e => e.PersonId, "IX_tblPersonInWeekend_PersonID");

                entity.HasIndex(e => e.ShopId, "IX_tblPersonInWeekend_ShopID");

                entity.Property(e => e.IdworkInWeekend).HasColumnName("IDWorkInWeekend");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Reason).HasColumnType("text");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.TblPersonInWeekends)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_WorkInWeekend_tblPersons");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblPersonInWeekends)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonInWeekend_tblShops");
            });

            modelBuilder.Entity<TblPlace>(entity =>
            {
                entity.HasKey(e => e.PlaceId)
                    .HasName("PK_tblPlace");

                entity.ToTable("tblPlaces");

                entity.HasIndex(e => e.ShopId, "IX_tblPlaces_ShopID");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.AddWhen).HasColumnType("datetime");

                entity.Property(e => e.AddWho).HasMaxLength(56);

                entity.Property(e => e.ModWhen).HasColumnType("datetime");

                entity.Property(e => e.ModWho).HasMaxLength(56);

                entity.Property(e => e.PlaceName)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("(N'Place')");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblPlaces)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tblPlaces_tblShops");
            });

            modelBuilder.Entity<TblShiftInWeekend>(entity =>
            {
                entity.HasKey(e => e.IdshopInWeekend);

                entity.ToTable("tblShiftInWeekend");

                entity.HasIndex(e => e.ShopId, "IX_tblShiftInWeekend_ShopID");

                entity.Property(e => e.IdshopInWeekend).HasColumnName("IDShopInWeekend");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblShiftInWeekends)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tblShiftInWeekend_tblShops");
            });

            modelBuilder.Entity<TblShop>(entity =>
            {
                entity.HasKey(e => e.ShopId)
                    .HasName("PK_Table_2");

                entity.ToTable("tblShops");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.Property(e => e.AddWhen).HasColumnType("datetime");

                entity.Property(e => e.AddWho).HasMaxLength(56);

                entity.Property(e => e.ChartCoutOfBreakdown).HasDefaultValueSql("((1))");

                entity.Property(e => e.ChartMtbf)
                    .HasColumnName("ChartMTBF")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChartMttr)
                    .HasColumnName("ChartMTTR")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChartPercentOfBreakdown).HasDefaultValueSql("((1))");

                entity.Property(e => e.MaxChartCoutOfBreakdown).HasDefaultValueSql("((10))");

                entity.Property(e => e.MaxChartMtbf)
                    .HasColumnName("MaxChartMTBF")
                    .HasDefaultValueSql("((10))");

                entity.Property(e => e.MaxChartMttr)
                    .HasColumnName("MaxChartMTTR")
                    .HasDefaultValueSql("((10))");

                entity.Property(e => e.MaxChartPercentOfBreakdown).HasDefaultValueSql("((10))");

                entity.Property(e => e.MinChartCoutOfBreakdown).HasDefaultValueSql("((1))");

                entity.Property(e => e.MinChartMtbf)
                    .HasColumnName("MinChartMTBF")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MinChartMttr)
                    .HasColumnName("MinChartMTTR")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MinChartPercentOfBreakdown).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModWhen).HasColumnType("datetime");

                entity.Property(e => e.ModWho).HasMaxLength(56);

                entity.Property(e => e.OfficeAlarm).HasDefaultValueSql("((99))");

                entity.Property(e => e.OfficeWarning).HasDefaultValueSql("((99))");

                entity.Property(e => e.ShopName).HasMaxLength(28);

                entity.Property(e => e.TechnicalAlarm).HasDefaultValueSql("((99))");

                entity.Property(e => e.TechnicalWarning).HasDefaultValueSql("((99))");
            });

            modelBuilder.Entity<TblUnavailable>(entity =>
            {
                entity.HasKey(e => e.UnavailableId);

                entity.ToTable("tblUnavailable");

                entity.HasIndex(e => e.PersonId, "IX_tblUnavailable_PersonID");

                entity.Property(e => e.UnavailableId).HasColumnName("UnavailableID");

                entity.Property(e => e.AbsentFrom).HasColumnType("date");

                entity.Property(e => e.AbsentTo).HasColumnType("date");

                entity.Property(e => e.DataOfSend).HasColumnType("date");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Reason).HasMaxLength(28);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.TblUnavailables)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_tblUnavailable_tblPersons");
            });

            modelBuilder.Entity<TblUserSelectedShop>(entity =>
            {
                entity.HasKey(e => e.DataId)
                    .HasName("PK_tblSelectedshop");

                entity.ToTable("tblUserSelectedShop");

                entity.HasIndex(e => e.ShopId, "IX_tblUserSelectedShop_ShopID");

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.ShopId).HasColumnName("ShopID");

                entity.Property(e => e.UserPc)
                    .HasMaxLength(64)
                    .HasColumnName("UserPC");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.TblUserSelectedShops)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_tblUserSelectedShop_tblShops");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
