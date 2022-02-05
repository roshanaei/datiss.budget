using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AppClaimTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClaimTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppDataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    XmlData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSqlCache",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(449)", maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSqlCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Constants",
                columns: table => new
                {
                    ConstantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ConstantKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constants", x => x.ConstantId);
                    table.ForeignKey(
                        name: "FK_Constants_Constants_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinanceYears",
                columns: table => new
                {
                    FinanceYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceYears", x => x.FinanceYearId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    OrgType = table.Column<int>(type: "int", nullable: false),
                    SewageStatus = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Organizations_Organizations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TablesFiledTitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TableName = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablesFiledTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablesFiledTitle_TablesFiledTitle_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TablesFiledTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    SubW = table.Column<int>(type: "int", nullable: false),
                    SubWs = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscription_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscription_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PhotoFileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastVisitDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEmailPublic = table.Column<bool>(type: "bit", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Constants_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchFeeAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UrbanAdjustmentFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WasteRateInWater = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WaterBranchingPerHousing = table.Column<int>(type: "int", nullable: false),
                    TubingCost = table.Column<int>(type: "int", nullable: false),
                    WaterPartnershipAmountDomestic = table.Column<int>(type: "int", nullable: false),
                    WaterPartnershipAmountNDomestic = table.Column<int>(type: "int", nullable: false),
                    WastePartnershipAmountDomestic = table.Column<int>(type: "int", nullable: false),
                    WastePartnershipAmountNDomestic = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11H = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11NH = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11HWs = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11NHWs = table.Column<int>(type: "int", nullable: false),
                    WsTubingCost = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchFeeAmount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchFeeAmount_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchFeeAmount_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchingRateIncrease",
                columns: table => new
                {
                    BranchingRateIncreaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WaterRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WasteRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WastePersentIncrease = table.Column<int>(type: "int", nullable: false),
                    FixAmountBusiness = table.Column<int>(type: "int", nullable: false),
                    CapacityFixAmount = table.Column<int>(type: "int", nullable: false),
                    WaterInstallRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WsInstalIncrease = table.Column<int>(type: "int", nullable: false),
                    WaterFixNote2 = table.Column<int>(type: "int", nullable: false),
                    WasteFixNote2 = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchingRateIncrease", x => x.BranchingRateIncreaseId);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cofficients",
                columns: table => new
                {
                    CofficientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CofficientTypeId = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cofficients", x => x.CofficientId);
                    table.ForeignKey(
                        name: "FK_Cofficients_Constants_CofficientTypeId",
                        column: x => x.CofficientTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cofficients_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cofficients_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsumeForcast",
                columns: table => new
                {
                    ConsumeForcastId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    CountUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUserForcast = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumeForcast", x => x.ConsumeForcastId);
                    table.ForeignKey(
                        name: "FK_ConsumeForcast_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcast_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcast_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcast_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsumeForcastWs",
                columns: table => new
                {
                    ConsumeForcastWsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    CountUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUserForcast = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumeForcastWs", x => x.ConsumeForcastWsId);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeCity",
                columns: table => new
                {
                    FeeCityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DomesticPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    NDomesticPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCity", x => x.FeeCityId);
                    table.ForeignKey(
                        name: "FK_FeeCity_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCity_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentNOperationals",
                columns: table => new
                {
                    ICNOId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    NOICTypeId = table.Column<int>(type: "int", nullable: false),
                    NOICPrice = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentNOperationals", x => x.ICNOId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentNOperationals_Constants_NOICTypeId",
                        column: x => x.NOICTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentNOperationals_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentNOperationals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentOperationals",
                columns: table => new
                {
                    ICOId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    ICOTypeId = table.Column<int>(type: "int", nullable: false),
                    CountH = table.Column<int>(type: "int", nullable: false),
                    PriceH = table.Column<int>(type: "int", nullable: false),
                    CostH = table.Column<int>(type: "int", nullable: false),
                    CountNH = table.Column<int>(type: "int", nullable: false),
                    PriceNH = table.Column<int>(type: "int", nullable: false),
                    CostNH = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentOperationals", x => x.ICOId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentOperationals_Constants_ICOTypeId",
                        column: x => x.ICOTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentOperationals_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentOperationals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentWH",
                columns: table => new
                {
                    IncomeCurrentWHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    TIncome = table.Column<int>(type: "int", nullable: false),
                    Diff_ConsWsVolume = table.Column<int>(type: "int", nullable: false),
                    Note2Income = table.Column<int>(type: "int", nullable: false),
                    WasteVolume = table.Column<int>(type: "int", nullable: false),
                    Note7Price = table.Column<int>(type: "int", nullable: false),
                    Note7Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentWH", x => x.IncomeCurrentWHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentWNH",
                columns: table => new
                {
                    IncomeCurrentWNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    ExcessIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<int>(type: "int", nullable: false),
                    Diff_ConsWsVolume = table.Column<int>(type: "int", nullable: false),
                    Note2Income = table.Column<int>(type: "int", nullable: false),
                    WasteVolume = table.Column<int>(type: "int", nullable: false),
                    Note7Price = table.Column<int>(type: "int", nullable: false),
                    Note7Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentWNH", x => x.IncomeCurrentWNHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentWsH",
                columns: table => new
                {
                    IncomeCurrentWsHID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    TIncome = table.Column<int>(type: "int", nullable: false),
                    Note7Price = table.Column<int>(type: "int", nullable: false),
                    Note7Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentWsH", x => x.IncomeCurrentWsHID);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentWsNH",
                columns: table => new
                {
                    IncomeCurrentWsNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    ExcessIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<int>(type: "int", nullable: false),
                    Note7Price = table.Column<int>(type: "int", nullable: false),
                    Note7Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCurrentWsNH", x => x.IncomeCurrentWsNHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeForcast",
                columns: table => new
                {
                    IncomeForcastId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    WaterInstllIncome = table.Column<int>(type: "int", nullable: false),
                    WaterBranchIncome = table.Column<int>(type: "int", nullable: false),
                    WaterNote2Income = table.Column<int>(type: "int", nullable: false),
                    WaterNote3Income = table.Column<int>(type: "int", nullable: false),
                    WNote11Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeForcast", x => x.IncomeForcastId);
                    table.ForeignKey(
                        name: "FK_IncomeForcast_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcast_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcast_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeForcastOthers",
                columns: table => new
                {
                    IFOId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    OIFTypeId = table.Column<int>(type: "int", nullable: false),
                    OIFCount = table.Column<int>(type: "int", nullable: false),
                    OIFPrice = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeForcastOthers", x => x.IFOId);
                    table.ForeignKey(
                        name: "FK_IncomeForcastOthers_Constants_OIFTypeId",
                        column: x => x.OIFTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastOthers_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastOthers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeForcastWs",
                columns: table => new
                {
                    IncomeForcastWsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    WasteInstallIncome = table.Column<int>(type: "int", nullable: false),
                    WasteBranchIncome = table.Column<int>(type: "int", nullable: false),
                    WasteNote3Income = table.Column<int>(type: "int", nullable: false),
                    WsNote11Income = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeForcastWs", x => x.IncomeForcastWsId);
                    table.ForeignKey(
                        name: "FK_IncomeForcastWs_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastWs_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastWs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NHCo",
                columns: table => new
                {
                    NHCoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    P1Capacity = table.Column<int>(type: "int", nullable: false),
                    FixCostCo = table.Column<int>(type: "int", nullable: false),
                    P1CostCo = table.Column<int>(type: "int", nullable: false),
                    P2CostCo = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHCo", x => x.NHCoId);
                    table.ForeignKey(
                        name: "FK_NHCo_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHCo_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesSplitTotal",
                columns: table => new
                {
                    SalesSplitTotalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WNumber = table.Column<int>(type: "int", nullable: false),
                    WUnit = table.Column<int>(type: "int", nullable: false),
                    WsNumber = table.Column<int>(type: "int", nullable: false),
                    WsUnit = table.Column<int>(type: "int", nullable: false),
                    WNumber_2 = table.Column<int>(type: "int", nullable: false),
                    WUnit_2 = table.Column<int>(type: "int", nullable: false),
                    WsNumber_2 = table.Column<int>(type: "int", nullable: false),
                    WsUnit_2 = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesSplitTotal", x => x.SalesSplitTotalId);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesSplitW_Y",
                columns: table => new
                {
                    SalesSplitWYID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WPipeDiameterId = table.Column<int>(type: "int", nullable: false),
                    NumberSales = table.Column<int>(type: "int", nullable: false),
                    UnitSales = table.Column<int>(type: "int", nullable: false),
                    AverageCapacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WInstallationCosts = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesSplitW_Y", x => x.SalesSplitWYID);
                    table.ForeignKey(
                        name: "FK_SalesSplitW_Y_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitW_Y_Constants_WPipeDiameterId",
                        column: x => x.WPipeDiameterId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitW_Y_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitW_Y_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesSplitWs_Y",
                columns: table => new
                {
                    SalesSplitWsYID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WsPipeDiameterId = table.Column<int>(type: "int", nullable: false),
                    NumberSales = table.Column<int>(type: "int", nullable: false),
                    UnitSales = table.Column<int>(type: "int", nullable: false),
                    AverageCapacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WsInstallationCosts = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesSplitWs_Y", x => x.SalesSplitWsYID);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Constants_WsPipeDiameterId",
                        column: x => x.WsPipeDiameterId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacity",
                columns: table => new
                {
                    UTACID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    AverageCapacityW = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWs = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypeAverageCapacity", x => x.UTACID);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacity_Y",
                columns: table => new
                {
                    UTACY_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    AverageCapacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWs = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypeAverageCapacity_Y", x => x.UTACY_ID);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Y_Constants_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Y_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Y_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WasteInstallFees",
                columns: table => new
                {
                    WasteInstallFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DWasteTypeId = table.Column<int>(type: "int", nullable: false),
                    WsInstallFee = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteInstallFees", x => x.WasteInstallFeeId);
                    table.ForeignKey(
                        name: "FK_WasteInstallFees_Constants_DWasteTypeId",
                        column: x => x.DWasteTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WasteInstallFees_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WasteInstallFees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaterInstallFees",
                columns: table => new
                {
                    WaterInstallFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DWaterTypeId = table.Column<int>(type: "int", nullable: false),
                    WInstallFee = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInstallFees", x => x.WaterInstallFeeId);
                    table.ForeignKey(
                        name: "FK_WaterInstallFees_Constants_DWaterTypeId",
                        column: x => x.DWaterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterInstallFees_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterInstallFees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WWsFee",
                columns: table => new
                {
                    WWsFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    P1Fee = table.Column<int>(type: "int", nullable: false),
                    P2Fee = table.Column<int>(type: "int", nullable: false),
                    P1Note3 = table.Column<int>(type: "int", nullable: false),
                    P2Note3 = table.Column<int>(type: "int", nullable: false),
                    P1Note7 = table.Column<int>(type: "int", nullable: false),
                    P2Note7 = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WWsFee", x => x.WWsFeeId);
                    table.ForeignKey(
                        name: "FK_WWsFee_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WWsFee_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WWsFee_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WWsFee_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParamType = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    ConstantKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportParams_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TableFieldId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Operation = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceEvaluation_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceEvaluation_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceEvaluation_TablesFiledTitle_TableFieldId",
                        column: x => x.TableFieldId,
                        principalTable: "TablesFiledTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HostName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AppUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserUsedPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HashedPassword = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserUsedPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserUsedPasswords_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDataProtectionKeys_FriendlyName",
                table: "AppDataProtectionKeys",
                column: "FriendlyName",
                unique: true,
                filter: "[FriendlyName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                schema: "dbo",
                table: "AppSqlCache",
                column: "ExpiresAtTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_UserId",
                table: "AppUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_OrganizationId",
                table: "AppUsers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_PositionId",
                table: "AppUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserUsedPasswords_UserId",
                table: "AppUserUsedPasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchFeeAmount_OrganizationId",
                table: "BranchFeeAmount",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchFeeAmount_YearId",
                table: "BranchFeeAmount",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_OrganizationId",
                table: "BranchingRateIncrease",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_UserTypeId",
                table: "BranchingRateIncrease",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_YearId",
                table: "BranchingRateIncrease",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Cofficients_CofficientTypeId",
                table: "Cofficients",
                column: "CofficientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cofficients_OrganizationId",
                table: "Cofficients",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cofficients_YearId",
                table: "Cofficients",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Constants_ParentId",
                table: "Constants",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcast_OrganizationId",
                table: "ConsumeForcast",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcast_UsageLayerId",
                table: "ConsumeForcast",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcast_UserTypeId",
                table: "ConsumeForcast",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcast_YearId",
                table: "ConsumeForcast",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_OrganizationId",
                table: "ConsumeForcastWs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_UsageLayerId",
                table: "ConsumeForcastWs",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_UserTypeId",
                table: "ConsumeForcastWs",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_YearId",
                table: "ConsumeForcastWs",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCity_OrganizationId",
                table: "FeeCity",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCity_YearId",
                table: "FeeCity",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentNOperationals_NOICTypeId",
                table: "IncomeCurrentNOperationals",
                column: "NOICTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentNOperationals_OrganizationId",
                table: "IncomeCurrentNOperationals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentNOperationals_YearId",
                table: "IncomeCurrentNOperationals",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentOperationals_ICOTypeId",
                table: "IncomeCurrentOperationals",
                column: "ICOTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentOperationals_OrganizationId",
                table: "IncomeCurrentOperationals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentOperationals_YearId",
                table: "IncomeCurrentOperationals",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_OrganizationId",
                table: "IncomeCurrentWH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_UsageLayerId",
                table: "IncomeCurrentWH",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_UserTypeId",
                table: "IncomeCurrentWH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_YearId",
                table: "IncomeCurrentWH",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_OrganizationId",
                table: "IncomeCurrentWNH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_UserTypeId",
                table: "IncomeCurrentWNH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_YearId",
                table: "IncomeCurrentWNH",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_OrganizationId",
                table: "IncomeCurrentWsH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_UsageLayerId",
                table: "IncomeCurrentWsH",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_UserTypeId",
                table: "IncomeCurrentWsH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_YearId",
                table: "IncomeCurrentWsH",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_OrganizationId",
                table: "IncomeCurrentWsNH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_UserTypeId",
                table: "IncomeCurrentWsNH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_YearId",
                table: "IncomeCurrentWsNH",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcast_OrganizationId",
                table: "IncomeForcast",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcast_UserTypeId",
                table: "IncomeForcast",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcast_YearId",
                table: "IncomeForcast",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastOthers_OIFTypeId",
                table: "IncomeForcastOthers",
                column: "OIFTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastOthers_OrganizationId",
                table: "IncomeForcastOthers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastOthers_YearId",
                table: "IncomeForcastOthers",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastWs_OrganizationId",
                table: "IncomeForcastWs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastWs_UserTypeId",
                table: "IncomeForcastWs",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastWs_YearId",
                table: "IncomeForcastWs",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_NHCo_OrganizationId",
                table: "NHCo",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHCo_YearId",
                table: "NHCo",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ParentId",
                table: "Organizations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceEvaluation_OrganizationId",
                table: "PerformanceEvaluation",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceEvaluation_TableFieldId",
                table: "PerformanceEvaluation",
                column: "TableFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceEvaluation_YearId",
                table: "PerformanceEvaluation",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportParams_ReportId",
                table: "ReportParams",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_OrganizationId",
                table: "SalesSplitTotal",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_UserTypeId",
                table: "SalesSplitTotal",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_YearId",
                table: "SalesSplitTotal",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitW_Y_OrganizationId",
                table: "SalesSplitW_Y",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitW_Y_UserTypeId",
                table: "SalesSplitW_Y",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitW_Y_WPipeDiameterId",
                table: "SalesSplitW_Y",
                column: "WPipeDiameterId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitW_Y_YearId",
                table: "SalesSplitW_Y",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_OrganizationId",
                table: "SalesSplitWs_Y",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_UserTypeId",
                table: "SalesSplitWs_Y",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_WsPipeDiameterId",
                table: "SalesSplitWs_Y",
                column: "WsPipeDiameterId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_YearId",
                table: "SalesSplitWs_Y",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UserTypeId",
                table: "Subscription",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_YearId",
                table: "Subscription",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_TablesFiledTitle_ParentId",
                table: "TablesFiledTitle",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_OrganizationId",
                table: "UserTypeAverageCapacity",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_UserTypeId",
                table: "UserTypeAverageCapacity",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_YearId",
                table: "UserTypeAverageCapacity",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_Y_OrganizationId",
                table: "UserTypeAverageCapacity_Y",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_Y_YearId",
                table: "UserTypeAverageCapacity_Y",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteInstallFees_DWasteTypeId",
                table: "WasteInstallFees",
                column: "DWasteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteInstallFees_OrganizationId",
                table: "WasteInstallFees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteInstallFees_YearId",
                table: "WasteInstallFees",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInstallFees_DWaterTypeId",
                table: "WaterInstallFees",
                column: "DWaterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInstallFees_OrganizationId",
                table: "WaterInstallFees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInstallFees_YearId",
                table: "WaterInstallFees",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_WWsFee_OrganizationId",
                table: "WWsFee",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WWsFee_UsageLayerId",
                table: "WWsFee",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_WWsFee_UserTypeId",
                table: "WWsFee",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WWsFee_YearId",
                table: "WWsFee",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClaimTypes");

            migrationBuilder.DropTable(
                name: "AppDataProtectionKeys");

            migrationBuilder.DropTable(
                name: "AppLogItems");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppSqlCache",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "AppUserUsedPasswords");

            migrationBuilder.DropTable(
                name: "BranchFeeAmount");

            migrationBuilder.DropTable(
                name: "BranchingRateIncrease");

            migrationBuilder.DropTable(
                name: "Cofficients");

            migrationBuilder.DropTable(
                name: "ConsumeForcast");

            migrationBuilder.DropTable(
                name: "ConsumeForcastWs");

            migrationBuilder.DropTable(
                name: "FeeCity");

            migrationBuilder.DropTable(
                name: "IncomeCurrentNOperationals");

            migrationBuilder.DropTable(
                name: "IncomeCurrentOperationals");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWH");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWNH");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWsH");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWsNH");

            migrationBuilder.DropTable(
                name: "IncomeForcast");

            migrationBuilder.DropTable(
                name: "IncomeForcastOthers");

            migrationBuilder.DropTable(
                name: "IncomeForcastWs");

            migrationBuilder.DropTable(
                name: "NHCo");

            migrationBuilder.DropTable(
                name: "PerformanceEvaluation");

            migrationBuilder.DropTable(
                name: "ReportParams");

            migrationBuilder.DropTable(
                name: "SalesSplitTotal");

            migrationBuilder.DropTable(
                name: "SalesSplitW_Y");

            migrationBuilder.DropTable(
                name: "SalesSplitWs_Y");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacity");

            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropTable(
                name: "WasteInstallFees");

            migrationBuilder.DropTable(
                name: "WaterInstallFees");

            migrationBuilder.DropTable(
                name: "WWsFee");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "TablesFiledTitle");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "FinanceYears");

            migrationBuilder.DropTable(
                name: "Constants");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
