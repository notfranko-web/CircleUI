using CircleUI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CircleUI.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<WebsiteProject> WebsiteProjects { get; set; } = null!;
    public DbSet<Page> Pages { get; set; } = null!;
    public DbSet<Component> Components { get; set; } = null!;
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<PublishedVersion> PublishedVersions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure JSON conversion for Component.Layout
        modelBuilder.Entity<Component>()
            .Property(c => c.Layout)
            .HasConversion(
                v => v,
                v => v,
                new ValueComparer<string>(
                    (d1, d2) => d1 == d2,
                    v => v.GetHashCode(),
                    v => v)
            );

        // Configure recursive component hierarchy
        modelBuilder.Entity<Component>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascade deletes

        // Indexes for performance
        modelBuilder.Entity<Page>()
            .HasIndex(p => new { p.ProjectId, p.Path })
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.UserId);
    }
}