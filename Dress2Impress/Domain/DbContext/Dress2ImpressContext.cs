using System;
using System.Collections.Generic;
using Dress2Impress.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.DBContext;

public partial class Dress2ImpressContext : DbContext
{
    public Dress2ImpressContext(DbContextOptions<Dress2ImpressContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClothingCategory> ClothingCategories { get; set; }

    public virtual DbSet<ClothingItem> ClothingItems { get; set; }

    public virtual DbSet<ClothingSubcategory> ClothingSubcategories { get; set; }

    public virtual DbSet<Colour> Colours { get; set; }

    public virtual DbSet<ColourRule> ColourRules { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Outfit> Outfits { get; set; }

    public virtual DbSet<OutfitClothingItem> OutfitClothingItems { get; set; }

    public virtual DbSet<Style> Styles { get; set; }

    public virtual DbSet<StyleRule> StyleRules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClothingItem>(entity =>
        {
            entity.HasOne(d => d.Colour).WithMany(p => p.ClothingItems).HasConstraintName("FK_Item_Colour");

            entity.HasOne(d => d.Location).WithMany(p => p.ClothingItems).HasConstraintName("FK_Item_Location");

            entity.HasOne(d => d.Style).WithMany(p => p.ClothingItems).HasConstraintName("FK_Item_Style");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.ClothingItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Subcategory");

            entity.HasOne(d => d.User).WithMany(p => p.ClothingItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_User");
        });

        modelBuilder.Entity<ClothingSubcategory>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.ClothingSubcategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subcategory_Category");
        });

        modelBuilder.Entity<Colour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Colours");
        });

        modelBuilder.Entity<ColourRule>(entity =>
        {
            entity.HasOne(d => d.Colour1).WithMany(p => p.ColourRuleColour1s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ColourRules_1");

            entity.HasOne(d => d.Colour2).WithMany(p => p.ColourRuleColour2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ColourRules_2");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07B3320BDA");
        });

        modelBuilder.Entity<Outfit>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Outfits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Outfit_User");
        });

        modelBuilder.Entity<OutfitClothingItem>(entity =>
        {
            entity.HasOne(d => d.Clothing).WithMany(p => p.OutfitClothingItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OutfitItems_Item");

            entity.HasOne(d => d.Outfit).WithMany(p => p.OutfitClothingItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OutfitItems_Outfit");
        });

        modelBuilder.Entity<StyleRule>(entity =>
        {
            entity.HasOne(d => d.Style1).WithMany(p => p.StyleRuleStyle1s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StyleRules_1");

            entity.HasOne(d => d.Style2).WithMany(p => p.StyleRuleStyle2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StyleRules_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
