using BuildOpsPlatform.RevitDataService.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildOpsPlatform.RevitDataService.DbContexts
{
    public class RevitDataDbContext : DbContext
    {
        public RevitDataDbContext(DbContextOptions<RevitDataDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Authors => Set<Author>();//
        public DbSet<Category> Categories => Set<Category>();//
        public DbSet<DesignOption> DesignOptions => Set<DesignOption>();//
        public DbSet<RvtDocument> RvtDocuments => Set<RvtDocument>();//
        public DbSet<RvtSnapshot> Snapshots => Set<RvtSnapshot>();//
        public DbSet<Error> Errors => Set<Error>();//
        public DbSet<Element> Elements => Set<Element>();//
        public DbSet<Grid> Grids => Set<Grid>();//
        public DbSet<Level> Levels => Set<Level>();//
        public DbSet<Material> Materials => Set<Material>();//
        public DbSet<Parameter> Parameters => Set<Parameter>();//
        public DbSet<Schedule> Schedules => Set<Schedule>();//
        public DbSet<Sheet> Sheets => Set<Sheet>();//
        public DbSet<Site> Sites => Set<Site>();//
        public DbSet<Stage> Stages => Set<Stage>();//
        public DbSet<View> Views => Set<View>();//
        public DbSet<Workset> Worksets => Set<Workset>();//

        public DbSet<ElementError> ElementErrors => Set<ElementError>();//
        public DbSet<ElementView> ElementViews => Set<ElementView>();//
        public DbSet<ElementMaterial> ElementMaterials => Set<ElementMaterial>();//
        public DbSet<ElementParameterValue> ParameterValues => Set<ElementParameterValue>();//

        //Projects
        public DbSet<ProjectsCategories> ProjectsCategories => Set<ProjectsCategories>();//

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Author
            modelBuilder.Entity<Author>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Documents)
                .WithOne(d => d.Author)
                .HasForeignKey(d => d.AuthorId);

            // Document\
            modelBuilder.Entity<RvtSnapshot>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<RvtSnapshot>()
                .HasMany(c => c.Categories)
                .WithOne(e => e.RvtSnapshot)
                .HasForeignKey(e => e.RvtSnapshotId);

            // Category
            modelBuilder.Entity<Category>()
                .HasKey(c => new { c.RvtSnapshotId, c.CategoryId });
            
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Elements)
                .WithOne(e => e.Category)
                .HasForeignKey(e => new { e.RvtSnapshotId, e.CategoryId })
                .HasPrincipalKey(c => new { c.RvtSnapshotId, c.CategoryId });

            // Element
            modelBuilder.Entity<Element>()
                .HasKey(e => new { e.RvtSnapshotId, e.ElementId });
            
            modelBuilder.Entity<Element>()
                .HasOne(e => e.RvtSnapshot)
                .WithMany(d => d.Elements)
                .HasForeignKey(e => e.RvtSnapshotId);
            
            modelBuilder.Entity<Element>()
                .HasOne(e => e.Level)
                .WithMany(l => l.Elements)
                .HasForeignKey(e => new { e.RvtSnapshotId, e.LevelId });
            
            modelBuilder.Entity<Element>()
                .HasOne(e => e.Workset)
                .WithMany(w => w.Elements)
                .HasForeignKey(e => new { e.RvtSnapshotId, e.WorksetId });
            
            modelBuilder.Entity<Element>()
                .HasOne(e => e.DesignOption)
                .WithMany(d => d.Elements)
                .HasForeignKey(e => new { e.RvtSnapshotId, e.DesignOptionId });
            
            modelBuilder.Entity<Element>()
                .HasOne(e => e.Stage)
                .WithMany(d => d.Elements)
                .HasForeignKey(e => new { e.RvtSnapshotId, e.StageId });

            // Level
            modelBuilder.Entity<Level>()
                .HasKey(l => new { l.RvtSnapshotId, l.LevelId });

            modelBuilder.Entity<Level>()
                .HasOne(l => l.RvtSnapshot)
                .WithMany(d => d.Levels)
                .HasForeignKey(l => l.RvtSnapshotId);

            // Grid
            modelBuilder.Entity<Grid>()
                .HasKey(g => new { g.RvtSnapshotId, g.GridId });

            modelBuilder.Entity<Grid>()
                .HasOne(g => g.RvtSnapshot)
                .WithMany(d => d.Grids)
                .HasForeignKey(g => g.RvtSnapshotId);

            // Workset
            modelBuilder.Entity<Workset>().
                HasKey(w => new { w.RvtSnapshotId, w.WorksetId });

            modelBuilder.Entity<Workset>()
                .HasOne(w => w.RvtSnapshot)
                .WithMany(d => d.Worksets)
                .HasForeignKey(w => w.RvtSnapshotId);

            // View
            modelBuilder.Entity<View>()
                .HasKey(v => new { v.RvtSnapshotId, v.ViewId });

            modelBuilder.Entity<View>()
                .HasOne(v => v.RvtSnapshot)
                .WithMany(d => d.Views)
                .HasForeignKey(v => v.RvtSnapshotId); 
            
            modelBuilder.Entity<Stage>()
                .HasKey(x => new { x.RvtSnapshotId, x.StageId });

            // DesignOption
            modelBuilder.Entity<DesignOption>()
                .HasKey(o => new { o.RvtSnapshotId, o.DesignOptionId });

            modelBuilder.Entity<DesignOption>()
                .HasOne(o => o.RvtSnapshot)
                .WithMany(d => d.DesignOptions)
                .HasForeignKey(o => o.RvtSnapshotId);

            // Material
            modelBuilder.Entity<Material>()
                .HasKey(m => new { m.RvtSnapshotId, m.MaterialId });

            modelBuilder.Entity<Material>()
                .HasOne(m => m.RvtSnapshot)
                .WithMany(d => d.Materials)
                .HasForeignKey(m => m.RvtSnapshotId);

            // Error
            modelBuilder.Entity<Error>()
                .HasKey(de => new { de.RvtSnapshotId, de.Id } );

            modelBuilder.Entity<Error>()
                .HasOne(e => e.RvtSnapshot)
                .WithMany(d => d.Errors)
                .HasForeignKey(e => e.RvtSnapshotId);

            // Parameter 
            modelBuilder.Entity<Parameter>()
                .HasKey(p => new { p.RvtSnapshotId, p.Id } );

            modelBuilder.Entity<Parameter>()
                .HasOne(p => p.RvtSnapshot)
                .WithMany(d => d.Parameters)
                .HasForeignKey(p => p.RvtSnapshotId);

            modelBuilder.Entity<Parameter>()
                .HasIndex(p => new { p.RvtSnapshotId, p.ParameterId })
                .IsUnique();
            modelBuilder.Entity<Parameter>()
                .HasIndex(p => new { p.RvtSnapshotId, p.ParameterGUID })
                .IsUnique();

            //many-to-many
            modelBuilder.Entity<ElementParameterValue>(ep =>
            {
                ep.HasKey(x => x.Id);

                ep.HasOne(x => x.Element)
                  .WithMany(e => e.Parameters)             // коллекция в Element
                  .HasForeignKey(x => new { x.RvtSnapshotId, x.ElementId })
                  .HasPrincipalKey(e => new { e.RvtSnapshotId, e.ElementId })
                  .OnDelete(DeleteBehavior.Cascade);

                ep.HasOne(x => x.Parameter)
                  .WithMany(p => p.ElementLinks)                  // коллекция в Parameter
                  .HasForeignKey(x => new { x.RvtSnapshotId, x.ParameterDbId })
                  .IsRequired(false)                              // ParameterId может быть null
                  .OnDelete(DeleteBehavior.Restrict);
            });

            //many-to-many
            modelBuilder.Entity<ElementView>().HasKey(ev => new
            {
                ev.RvtSnapshotId,
                ev.ElementId,
                ev.ViewId
            });
            modelBuilder.Entity<ElementView>()
                .HasOne(ev => ev.Element)
                .WithMany(e => e.ElementViews)
                .HasForeignKey(ev => new { ev.RvtSnapshotId, ev.ElementId });
            modelBuilder.Entity<ElementView>()
                .HasOne(ev => ev.View)
                .WithMany(v => v.ElementViews)
                .HasForeignKey(ev => new { ev.RvtSnapshotId, ev.ViewId });

            //many-to-many
            modelBuilder.Entity<ElementMaterial>().HasKey(em => new { em.RvtSnapshotId, em.ElementId, em.MaterialId });
            modelBuilder.Entity<ElementMaterial>()
                .HasOne(em => em.Element)
                .WithMany(e => e.ElementMaterials)
                .HasForeignKey(em => new { em.RvtSnapshotId, em.ElementId });
            modelBuilder.Entity<ElementMaterial>()
                .HasOne(em => em.Material)
                .WithMany(m => m.ElementMaterials)
                .HasForeignKey(em => new { em.RvtSnapshotId, em.MaterialId });

            //many-to-many
            modelBuilder.Entity<ElementError>()
                .HasKey(ee => new { ee.RvtSnapshotId, ee.Id });

            modelBuilder.Entity<ElementError>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ElementError>()
                .HasOne(ee => ee.Element)
                .WithMany(e => e.Errors)
                .HasForeignKey(ee => new { ee.RvtSnapshotId, ee.ElementId });

            modelBuilder.Entity<ElementError>()
                .HasOne(ee => ee.Error)
                .WithMany(de => de.Errors)
                .HasForeignKey(ee => new { ee.RvtSnapshotId, ee.ErrorId });

            //many-to-many
            modelBuilder.Entity<ProjectsCategories>()
                .HasKey(pc => new { pc.ProjectId, pc.CategoryId });
        }
    }
}
