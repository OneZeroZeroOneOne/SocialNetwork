using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ValueGenerators;

namespace SocialNetwork.Dal.Context
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

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostComment> PostComment { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPost> UserPost { get; set; }

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

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ImgUrl).IsRequired();

                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ImgUrl).IsRequired();

                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.CommentId })
                    .HasName("PostComment_pk");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.PostComment)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PostComment_fk1");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PostComment_fk0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<UserPost>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("UserPost_pk");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserPost)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserPost_fk1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPost)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserPost_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
