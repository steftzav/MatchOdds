using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MatchOdds.Models;

public partial class MatchOddsContext : DbContext
{
    public MatchOddsContext()
    {
    }

    public MatchOddsContext(DbContextOptions<MatchOddsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchOdd> MatchOdds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MatchOddsContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatchOdd>(entity =>
        {
            entity.HasIndex(e => e.MatchId, "IX_MatchOdds_MatchId");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchOdds).HasForeignKey(d => d.MatchId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
