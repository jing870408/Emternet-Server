using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SDServer.Models
{
    public partial class SDContext : DbContext
    {
        public SDContext()
        {
        }

        public SDContext(DbContextOptions<SDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountEmo> AccountEmo { get; set; }
        public virtual DbSet<ChatRoom> ChatRoom { get; set; }
        public virtual DbSet<ChatRoomList> ChatRoomList { get; set; }
        public virtual DbSet<EmoVarData> EmoVarData { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<FriendshipGroup> FriendshipGroup { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<OriVarData> OriVarData { get; set; }
        public virtual DbSet<SongListF> SongListF { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AccountEmo>(entity =>
            {
                entity.HasKey(e => e.Account);

                entity.Property(e => e.Account)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Mood)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => e.Idx);

                entity.Property(e => e.Idx).HasColumnName("idx");

                entity.Property(e => e.SendContent)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Sender)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sendtime).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ChatRoom)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatRoom_ChatRoomList");
            });

            modelBuilder.Entity<ChatRoomList>(entity =>
            {
                entity.HasKey(e => e.Idx);

                entity.Property(e => e.Idx).HasColumnName("idx");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Person)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmoVarData>(entity =>
            {
                entity.HasKey(e => e.Idx);

                entity.Property(e => e.Idx).HasColumnName("idx");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MoodFinal)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.評分)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(e => e.Idx);

                entity.Property(e => e.Idx).HasColumnName("idx");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mood)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Friendship)
                    .HasPrincipalKey(p => p.GroupId)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Friendship_FriendshipGroup");
            });

            modelBuilder.Entity<FriendshipGroup>(entity =>
            {
                entity.HasKey(e => e.Account);

                entity.HasIndex(e => e.GroupId)
                    .HasName("UQ_FriendshipGroup")
                    .IsUnique();

                entity.Property(e => e.Account)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Members>(entity =>
            {
                entity.HasKey(e => e.Account);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ_Members")
                    .IsUnique();

                entity.Property(e => e.Account)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OriVarData>(entity =>
            {
                entity.HasKey(e => e.Idx);

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<SongListF>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Music)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.YtUrl)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
