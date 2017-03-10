namespace Kontur.GameStats.Application.App_Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ConturDataModel : DbContext
    {
        public ConturDataModel()
            : base("name=ConturDataModel")
        {
        }

        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<MatchesPlayers> MatchesPlayers { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<PlayerStat> PlayerStat { get; set; }
        public virtual DbSet<PlayerStatGameMaps> PlayerStatGameMaps { get; set; }
        public virtual DbSet<PlayerStatGameModes> PlayerStatGameModes { get; set; }
        public virtual DbSet<PlayerStatServers> PlayerStatServers { get; set; }
        public virtual DbSet<ServerGameModes> ServerGameModes { get; set; }
        public virtual DbSet<Servers> Servers { get; set; }
        public virtual DbSet<ServerStatGameMaps> ServerStatGameMaps { get; set; }
        public virtual DbSet<ServerStatGameModes> ServerStatGameModes { get; set; }
        public virtual DbSet<ServerStats> ServerStats { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matches>()
                .Property(e => e.gamemode)
                .IsUnicode(false);

            modelBuilder.Entity<Matches>()
                .Property(e => e.map)
                .IsUnicode(false);

            modelBuilder.Entity<Players>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Players>()
                .HasMany(e => e.PlayerStatServers)
                .WithRequired(e => e.Players)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Players>()
                .HasOptional(e => e.PlayerStat)
                .WithRequired(e => e.Players)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PlayerStat>()
                .HasMany(e => e.PlayerStatGameMaps)
                .WithRequired(e => e.PlayerStat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayerStat>()
                .HasMany(e => e.PlayerStatGameModes)
                .WithRequired(e => e.PlayerStat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayerStatGameMaps>()
                .Property(e => e.gamemode)
                .IsUnicode(false);

            modelBuilder.Entity<PlayerStatGameModes>()
                .Property(e => e.gamemode)
                .IsUnicode(false);

            modelBuilder.Entity<ServerGameModes>()
                .Property(e => e.gamemode)
                .IsUnicode(false);

            modelBuilder.Entity<Servers>()
                .Property(e => e.endpoint)
                .IsUnicode(false);

            modelBuilder.Entity<Servers>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Servers>()
                .HasMany(e => e.PlayerStatServers)
                .WithRequired(e => e.Servers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servers>()
                .HasMany(e => e.ServerGameModes)
                .WithRequired(e => e.Servers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servers>()
                .HasOptional(e => e.ServerStats)
                .WithRequired(e => e.Servers)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ServerStatGameMaps>()
                .Property(e => e.map)
                .IsUnicode(false);

            modelBuilder.Entity<ServerStatGameModes>()
                .Property(e => e.gamemode)
                .IsUnicode(false);

            modelBuilder.Entity<ServerStats>()
                .HasMany(e => e.ServerStatGameMaps)
                .WithRequired(e => e.ServerStats)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServerStats>()
                .HasMany(e => e.ServerStatGameModes)
                .WithRequired(e => e.ServerStats)
                .WillCascadeOnDelete(false);
        }
    }
}
