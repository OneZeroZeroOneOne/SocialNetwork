using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SocialNetwork.WebApi.Models
{
    public partial class PublicContext : DbContext
    {
        public PublicContext()
        {
        }

        public PublicContext(DbContextOptions<PublicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coments> Coments { get; set; }
        public virtual DbSet<PostComents> PostComents { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<UserPosts> UserPosts { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=82.146.37.127;Database=postgres;Username=postgres;Password=werdwerd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Coments>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Img).IsRequired();
            });

            modelBuilder.Entity<PostComents>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Coment)
                    .WithMany(p => p.PostComents)
                    .HasForeignKey(d => d.ComentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PostComents_fk1");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComents)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PostComents_fk0");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Img).IsRequired();

                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<UserPosts>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserPosts_fk1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserPosts_fk0");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Users_Name_key")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
