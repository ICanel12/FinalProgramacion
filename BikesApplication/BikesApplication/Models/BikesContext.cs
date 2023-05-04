using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BikesApplication.Models;

public partial class BikesContext : DbContext
{
    public BikesContext()
    {
    }

    public BikesContext(DbContextOptions<BikesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bike> Bikes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            var connectionString = configuration.GetConnectionString("DBBikes");

            optionsBuilder.UseMySQL(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bike>(entity =>
        {
            entity.HasKey(e => e.IdBike).HasName("PRIMARY");

            entity.ToTable("bike");

            entity.Property(e => e.IdBike).HasColumnName("id_bike");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .HasColumnName("brand");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumberDishes).HasColumnName("number_dishes");
            entity.Property(e => e.NumberSprockets).HasColumnName("number_sprockets");
            entity.Property(e => e.Size)
                .HasPrecision(10)
                .HasColumnName("size");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
