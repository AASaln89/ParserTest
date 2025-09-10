using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BuildOpsPlatform.RevitDataService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RvtDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProjectCode = table.Column<string>(type: "text", nullable: true),
                    ProjectName = table.Column<string>(type: "text", nullable: true),
                    Stage = table.Column<string>(type: "text", nullable: true),
                    Building = table.Column<string>(type: "text", nullable: true),
                    Discipline = table.Column<string>(type: "text", nullable: true),
                    AppVersion = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<int>(type: "integer", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RvtDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RvtDocuments_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocumentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Snapshots_RvtDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "RvtDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryType = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => new { x.RvtSnapshotId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_Categories_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignOptions",
                columns: table => new
                {
                    DesignOptionId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    DesignOptionUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignOptions", x => new { x.RvtSnapshotId, x.DesignOptionId });
                    table.ForeignKey(
                        name: "FK_DesignOptions_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ErrorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => new { x.RvtSnapshotId, x.Id });
                    table.ForeignKey(
                        name: "FK_Errors_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grids",
                columns: table => new
                {
                    GridId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    GridUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grids", x => new { x.RvtSnapshotId, x.GridId });
                    table.ForeignKey(
                        name: "FK_Grids_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ElevationValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => new { x.RvtSnapshotId, x.LevelId });
                    table.ForeignKey(
                        name: "FK_Levels_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => new { x.RvtSnapshotId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_Materials_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterId = table.Column<int>(type: "integer", nullable: true),
                    ParameterGUID = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => new { x.RvtSnapshotId, x.Id });
                    table.ForeignKey(
                        name: "FK_Parameters_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sheets",
                columns: table => new
                {
                    SheetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SheetUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheets", x => x.SheetId);
                    table.ForeignKey(
                        name: "FK_Sheets_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    BasePointElevetionValue = table.Column<string>(type: "text", nullable: true),
                    BasePointEastWest = table.Column<string>(type: "text", nullable: true),
                    BasePointNorthSouth = table.Column<string>(type: "text", nullable: true),
                    BasePointAngle = table.Column<string>(type: "text", nullable: true),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.SiteId);
                    table.ForeignKey(
                        name: "FK_Sites_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    StageId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => new { x.RvtSnapshotId, x.StageId });
                    table.ForeignKey(
                        name: "FK_Stages_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    ViewId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => new { x.RvtSnapshotId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_Views_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worksets",
                columns: table => new
                {
                    WorksetId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorksetUniqueId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worksets", x => new { x.RvtSnapshotId, x.WorksetId });
                    table.ForeignKey(
                        name: "FK_Worksets_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsCategories",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CategoryRvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId1 = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsCategories", x => new { x.ProjectId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectsCategories_Categories_CategoryRvtSnapshotId_Categor~",
                        columns: x => new { x.CategoryRvtSnapshotId, x.CategoryId1 },
                        principalTable: "Categories",
                        principalColumns: new[] { "RvtSnapshotId", "CategoryId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsCategories_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleFilter",
                columns: table => new
                {
                    ScheduleFilterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleUniqueId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    ParameterRefId = table.Column<int>(type: "integer", nullable: true),
                    ParameterRefGuid = table.Column<string>(type: "text", nullable: true),
                    SystemParameterRvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemParameterId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilterType = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleFilter", x => x.ScheduleFilterId);
                    table.ForeignKey(
                        name: "FK_ScheduleFilter_Parameters_SystemParameterRvtSnapshotId_Syst~",
                        columns: x => new { x.SystemParameterRvtSnapshotId, x.SystemParameterId },
                        principalTable: "Parameters",
                        principalColumns: new[] { "RvtSnapshotId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleFilter_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleFilter_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ElementId = table.Column<int>(type: "integer", nullable: false),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementUniqueId = table.Column<string>(type: "text", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LevelId = table.Column<int>(type: "integer", nullable: true),
                    WorksetId = table.Column<int>(type: "integer", nullable: true),
                    StageId = table.Column<int>(type: "integer", nullable: true),
                    DesignOptionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => new { x.RvtSnapshotId, x.ElementId });
                    table.ForeignKey(
                        name: "FK_Elements_Categories_RvtSnapshotId_CategoryId",
                        columns: x => new { x.RvtSnapshotId, x.CategoryId },
                        principalTable: "Categories",
                        principalColumns: new[] { "RvtSnapshotId", "CategoryId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_DesignOptions_RvtSnapshotId_DesignOptionId",
                        columns: x => new { x.RvtSnapshotId, x.DesignOptionId },
                        principalTable: "DesignOptions",
                        principalColumns: new[] { "RvtSnapshotId", "DesignOptionId" });
                    table.ForeignKey(
                        name: "FK_Elements_Levels_RvtSnapshotId_LevelId",
                        columns: x => new { x.RvtSnapshotId, x.LevelId },
                        principalTable: "Levels",
                        principalColumns: new[] { "RvtSnapshotId", "LevelId" });
                    table.ForeignKey(
                        name: "FK_Elements_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_Stages_RvtSnapshotId_StageId",
                        columns: x => new { x.RvtSnapshotId, x.StageId },
                        principalTable: "Stages",
                        principalColumns: new[] { "RvtSnapshotId", "StageId" });
                    table.ForeignKey(
                        name: "FK_Elements_Worksets_RvtSnapshotId_WorksetId",
                        columns: x => new { x.RvtSnapshotId, x.WorksetId },
                        principalTable: "Worksets",
                        principalColumns: new[] { "RvtSnapshotId", "WorksetId" });
                });

            migrationBuilder.CreateTable(
                name: "ElementErrors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementId = table.Column<int>(type: "integer", nullable: true),
                    ErrorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementErrors", x => new { x.RvtSnapshotId, x.Id });
                    table.ForeignKey(
                        name: "FK_ElementErrors_Elements_RvtSnapshotId_ElementId",
                        columns: x => new { x.RvtSnapshotId, x.ElementId },
                        principalTable: "Elements",
                        principalColumns: new[] { "RvtSnapshotId", "ElementId" });
                    table.ForeignKey(
                        name: "FK_ElementErrors_Errors_RvtSnapshotId_ErrorId",
                        columns: x => new { x.RvtSnapshotId, x.ErrorId },
                        principalTable: "Errors",
                        principalColumns: new[] { "RvtSnapshotId", "Id" });
                    table.ForeignKey(
                        name: "FK_ElementErrors_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementMaterials",
                columns: table => new
                {
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementId = table.Column<int>(type: "integer", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementMaterials", x => new { x.RvtSnapshotId, x.ElementId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_ElementMaterials_Elements_RvtSnapshotId_ElementId",
                        columns: x => new { x.RvtSnapshotId, x.ElementId },
                        principalTable: "Elements",
                        principalColumns: new[] { "RvtSnapshotId", "ElementId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementMaterials_Materials_RvtSnapshotId_MaterialId",
                        columns: x => new { x.RvtSnapshotId, x.MaterialId },
                        principalTable: "Materials",
                        principalColumns: new[] { "RvtSnapshotId", "MaterialId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementMaterials_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementViews",
                columns: table => new
                {
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementId = table.Column<int>(type: "integer", nullable: false),
                    ViewId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: true),
                    SheetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementViews", x => new { x.RvtSnapshotId, x.ElementId, x.ViewId });
                    table.ForeignKey(
                        name: "FK_ElementViews_Elements_RvtSnapshotId_ElementId",
                        columns: x => new { x.RvtSnapshotId, x.ElementId },
                        principalTable: "Elements",
                        principalColumns: new[] { "RvtSnapshotId", "ElementId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementViews_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId");
                    table.ForeignKey(
                        name: "FK_ElementViews_Sheets_SheetId",
                        column: x => x.SheetId,
                        principalTable: "Sheets",
                        principalColumn: "SheetId");
                    table.ForeignKey(
                        name: "FK_ElementViews_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementViews_Views_RvtSnapshotId_ViewId",
                        columns: x => new { x.RvtSnapshotId, x.ViewId },
                        principalTable: "Views",
                        principalColumns: new[] { "RvtSnapshotId", "ViewId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementId = table.Column<int>(type: "integer", nullable: false),
                    ParameterId = table.Column<int>(type: "integer", nullable: true),
                    ParameterGUID = table.Column<Guid>(type: "uuid", nullable: true),
                    ParameterDbId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsTypeParameter = table.Column<bool>(type: "boolean", nullable: false),
                    StorageType = table.Column<int>(type: "integer", nullable: true),
                    UnitType = table.Column<string>(type: "text", nullable: true),
                    IsShared = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IsProject = table.Column<bool>(type: "boolean", nullable: false),
                    HasValue = table.Column<bool>(type: "boolean", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    RvtSnapshotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterValues_Elements_RvtSnapshotId_ElementId",
                        columns: x => new { x.RvtSnapshotId, x.ElementId },
                        principalTable: "Elements",
                        principalColumns: new[] { "RvtSnapshotId", "ElementId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterValues_Parameters_RvtSnapshotId_ParameterDbId",
                        columns: x => new { x.RvtSnapshotId, x.ParameterDbId },
                        principalTable: "Parameters",
                        principalColumns: new[] { "RvtSnapshotId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParameterValues_Snapshots_RvtSnapshotId",
                        column: x => x.RvtSnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementErrors_RvtSnapshotId_ElementId",
                table: "ElementErrors",
                columns: new[] { "RvtSnapshotId", "ElementId" });

            migrationBuilder.CreateIndex(
                name: "IX_ElementErrors_RvtSnapshotId_ErrorId",
                table: "ElementErrors",
                columns: new[] { "RvtSnapshotId", "ErrorId" });

            migrationBuilder.CreateIndex(
                name: "IX_ElementMaterials_RvtSnapshotId_MaterialId",
                table: "ElementMaterials",
                columns: new[] { "RvtSnapshotId", "MaterialId" });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_RvtSnapshotId_CategoryId",
                table: "Elements",
                columns: new[] { "RvtSnapshotId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_RvtSnapshotId_DesignOptionId",
                table: "Elements",
                columns: new[] { "RvtSnapshotId", "DesignOptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_RvtSnapshotId_LevelId",
                table: "Elements",
                columns: new[] { "RvtSnapshotId", "LevelId" });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_RvtSnapshotId_StageId",
                table: "Elements",
                columns: new[] { "RvtSnapshotId", "StageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_RvtSnapshotId_WorksetId",
                table: "Elements",
                columns: new[] { "RvtSnapshotId", "WorksetId" });

            migrationBuilder.CreateIndex(
                name: "IX_ElementViews_RvtSnapshotId_ViewId",
                table: "ElementViews",
                columns: new[] { "RvtSnapshotId", "ViewId" });

            migrationBuilder.CreateIndex(
                name: "IX_ElementViews_ScheduleId",
                table: "ElementViews",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementViews_SheetId",
                table: "ElementViews",
                column: "SheetId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_RvtSnapshotId_ParameterGUID",
                table: "Parameters",
                columns: new[] { "RvtSnapshotId", "ParameterGUID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_RvtSnapshotId_ParameterId",
                table: "Parameters",
                columns: new[] { "RvtSnapshotId", "ParameterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterValues_RvtSnapshotId_ElementId",
                table: "ParameterValues",
                columns: new[] { "RvtSnapshotId", "ElementId" });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterValues_RvtSnapshotId_ParameterDbId",
                table: "ParameterValues",
                columns: new[] { "RvtSnapshotId", "ParameterDbId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsCategories_CategoryRvtSnapshotId_CategoryId1",
                table: "ProjectsCategories",
                columns: new[] { "CategoryRvtSnapshotId", "CategoryId1" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsCategories_RvtSnapshotId",
                table: "ProjectsCategories",
                column: "RvtSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_RvtDocuments_AuthorId",
                table: "RvtDocuments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleFilter_RvtSnapshotId",
                table: "ScheduleFilter",
                column: "RvtSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleFilter_ScheduleId",
                table: "ScheduleFilter",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleFilter_SystemParameterRvtSnapshotId_SystemParameter~",
                table: "ScheduleFilter",
                columns: new[] { "SystemParameterRvtSnapshotId", "SystemParameterId" });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RvtSnapshotId",
                table: "Schedules",
                column: "RvtSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_RvtSnapshotId",
                table: "Sheets",
                column: "RvtSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_RvtSnapshotId",
                table: "Sites",
                column: "RvtSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_DocumentId",
                table: "Snapshots",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementErrors");

            migrationBuilder.DropTable(
                name: "ElementMaterials");

            migrationBuilder.DropTable(
                name: "ElementViews");

            migrationBuilder.DropTable(
                name: "Grids");

            migrationBuilder.DropTable(
                name: "ParameterValues");

            migrationBuilder.DropTable(
                name: "ProjectsCategories");

            migrationBuilder.DropTable(
                name: "ScheduleFilter");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Sheets");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DesignOptions");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Worksets");

            migrationBuilder.DropTable(
                name: "Snapshots");

            migrationBuilder.DropTable(
                name: "RvtDocuments");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
