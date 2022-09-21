using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System;
using Datiss.Budget.Entities;
using Datiss.Budget.DataLayer.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Datiss.Budget.Common.EFCoreToolkit;
using Datiss.Budget.DataLayer.Configurations;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.DataLayer.Context
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// plus http://www.dotnettips.info/post/2491
    /// </summary>
    public class ApplicationDbContext :
        IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        #region BaseClass

        public virtual DbSet<AppLogItem> AppLogItems { get; set; }
        public virtual DbSet<AppSqlCache> AppSqlCache { get; set; }
        public virtual DbSet<AppDataProtectionKey> AppDataProtectionKeys { get; set; }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Commit();
        }

        public override void Dispose()
        {
            _transaction?.Dispose();
            base.Dispose();
        }

        public void ExecuteSqlInterpolatedCommand(FormattableString query)
        {
            Database.ExecuteSqlInterpolated(query);
        }

        public void ExecuteSqlRawCommand(string query, params object[] parameters)
            => Database.ExecuteSqlRaw(query, parameters);
        
        public async Task ExecuteSqlRawCommandAsync(string query, params object[] parameters) 
            => await Database.ExecuteSqlRawAsync(query, parameters);
        
        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = this.Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default;
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return this.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges(); //NOTE: changeTracker.Entries<T>() will call it automatically.

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public async Task<T> ExecuteScalar<T>(string sql, params object[] parameters) {
            var con = Database.GetDbConnection();
            using (var command = con.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                con.OpenAsync();

                var result = command.ExecuteScalar();

                if (result is DBNull)
                    return default(T);

                command.Parameters.Clear();

                return (T)result;
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters) {
            var con = this.Database.GetDbConnection();
            using (var command = con.CreateCommand()) {
                command.CommandText = sql;
                if (parameters != null) {
                    command.Parameters.AddRange(parameters);
                }
                await con.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                if (result is DBNull)
                    return default(T);

                command.Parameters.Clear();

                return (T)result;
            }
        }

        private void beforeSaveTriggers()
        {
            validateEntities();
            setShadowProperties();
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var props = this.GetService<IHttpContextAccessor>()?.GetShadowProperties();
            ChangeTracker.SetAuditableEntityPropertyValues(props);
        }

        private void validateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
                var loggerFactory = this.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        #endregion


        public virtual DbSet<FinanceYear> FinanceYears { set; get; }
        public virtual DbSet<Constant> Constants { set; get; }
        public virtual DbSet<Organization> Organizations { set; get; }

        public virtual DbSet<WaterInstallFee> WaterInstallFees { set; get; }
        public virtual DbSet<WasteInstallFee> WasteInstallFees { set; get; }
        public virtual DbSet<WaterSalesSplit> WaterSalesSplits { set; get; }
        public virtual DbSet<BranchFeeAmount> BranchFeeAmounts { get; set; }
        public virtual DbSet<WasteSalesSplit> WasteSalesSplits { get; set; }
        public virtual DbSet<TablesFiledTitle> TablesFiledTitles { get; set; }
        public virtual DbSet<AverageContractedCapacityNHUses> AverageContractedCapacityNHUses { get; set; }
        public virtual DbSet<IncomeCurrentCofficient> IncomeCurrentCofficients { get; set; }
        public virtual DbSet<CostForcastTransferW> CostForcastTransferW { get; set; }
        public virtual DbSet<CostForcastTransferWs> CostForcastTransferWs { get; set; }
        public virtual DbSet<CostForcastConstructionW> CostForcastConstructionW { get; set; }
        public virtual DbSet<CostForcastConstructionWs> CostForcastConstructionWs { get; set; }
        public virtual DbSet<CostCurrentPersonel> CostCurrentPersonel { get; set; }
        public virtual DbSet<CostCurrentRawMaterial> CostCurrentRawMaterial { get; set; }
        public virtual DbSet<CostForcastBuy> CostForcastBuy { get; set; }
        public virtual DbSet<CostCurrentReport> CostCurrentReports { get; set; }
        public virtual DbSet<BudgetSourceReport> BudgetSourceReports { get; set; }
        public virtual DbSet<IncomeCurrentWsNH> IncomeCurrentWsNH { get; set; }
        public virtual DbSet<CostForcastFinance> CostForcastFinance { get; set; }
        public virtual DbSet<CostCurrentWaterSourcePrice> CostCurrentWaterSourcePrice { get; set; }
        public virtual DbSet<CostCurrentPrescriptionBaseInfo> CostCurrentPrescriptionBaseInfo { get; set; }
        public virtual DbSet<CostForcastPipingW> CostForcastPipingW { get; set; }
        public virtual DbSet<CostForcastPipingWs> CostForcastPipingWs { get; set; }
        public virtual DbSet<CostForcastBuyDescription> CostForcastBuyDescription { get; set; }
        public virtual DbSet<CostForcastConsumptionReport> CostForcastConsumptionReport { get; set; }

        public virtual DbSet<CostCurrentProfitLossReport> CostCurrentProfitLossReport { get; set; }







        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            // Adds all of the ASP.NET Core Identity related mappings at once.
            builder.AddCustomIdentityMappings(this.GetService<IOptionsSnapshot<SiteSettings>>()?.Value);

            // Custom application mappings
            builder.SetDecimalPrecision();
            builder.AddDateTimeUtcKindConverter();

            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();

            //TODO : Add all mapping configuration
            builder
                .ApplyConfiguration(new WaterInstallFeeConfiguration())
                .ApplyConfiguration(new FinanceYearConfiguration())
                .ApplyConfiguration(new WasteInstallFeeConfiguration())
                .ApplyConfiguration(new WasteSalesSplitConfiguration())
                .ApplyConfiguration(new WaterSalesSplitConfiguration())
                .ApplyConfiguration(new ReportConfiguration())
                .ApplyConfiguration(new ReportParamConfiguration())
                .ApplyConfiguration(new RoleConfiguration())
                .ApplyConfiguration(new AppClaimTypeConfiguration())
                .ApplyConfiguration(new IncomeCurrentCofficientConfiguration())
                .ApplyConfiguration(new CostForcastTransferWConfiguration())
                .ApplyConfiguration(new CostForcastTransferWsConfiguration())
                .ApplyConfiguration(new CostForcastConstructionWConfiguration())
                .ApplyConfiguration(new CostForcastConstructionWsConfiguration())
                .ApplyConfiguration(new CostCurrentPersonelConfiguration())
                .ApplyConfiguration(new CostCurrentRawMaterialConfiguration())
                .ApplyConfiguration(new CostForcastBuyConfiguration())
                .ApplyConfiguration(new CostCurrentReportConfiguration())
                .ApplyConfiguration(new BudgetSourceReportConfiguration())
                .ApplyConfiguration(new IncomeCurrentWsNHConfiguration())
                .ApplyConfiguration(new CostForcastFinanceConfiguration())
                .ApplyConfiguration(new CostCurrentWaterSourcePriceConfiguration())
                .ApplyConfiguration(new CostCurrentPrescriptionBaseInfoConfiguration())
                .ApplyConfiguration(new CostForcastPipingWConfiguration())
                .ApplyConfiguration(new CostForcastPipingWsConfiguration())
                .ApplyConfiguration(new CostForcastBuyDescriptionConfiguration())
                .ApplyConfiguration(new CostForcastConsumptionReportConfiguration())
                .ApplyConfiguration(new CostCurrentProfitLossReportConfiguration());
        }
    }
}