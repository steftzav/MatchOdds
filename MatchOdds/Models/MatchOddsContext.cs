using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MatchOdds.Models;

public partial class MatchOddsContext : DbContext
{
    private readonly string _connectionString;

    public MatchOddsContext()
    {
    }   

    public MatchOddsContext(DbContextOptions<MatchOddsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchOdds> MatchOdds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThreeLetterCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TwoLetterCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.CreatedAt)
                 .HasDefaultValue(DateTime.Now);

            entity.HasMany(e => e.Ipaddresses)
                .WithOne(ip => ip.Country)
                .HasForeignKey(e => e.CountryId);
        });

        modelBuilder.Entity<Ipaddress>(entity =>
        {
            entity.ToTable("IPAddresses");

            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValue(DateTime.Now);

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValue(DateTime.Now);

            entity.ToTable(tb => tb.HasTrigger("AutoUpdate"));
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}