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
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserSecurity> UserSecurity { get; set; }
        public virtual DbSet<UserConfirmationToken> UserConfirmationToken { get; set; }
        public virtual DbSet<ReactionComment> ReactionComment { get; set; }
        public virtual DbSet<ReactionPost> ReactionPost { get; set; }
        public virtual DbSet<Reaction> Reaction { get; set; }

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

                entity.Property(x => x.Date).HasValueGenerator<CurrentDateTimeGenerator>();

                entity.Property(e => e.Text).IsRequired();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_fk1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_fk0");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.Property(e => e.Text).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Post_fk0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.HasOne(x => x.UserSecurity)
                    .WithOne(x => x.User);
            });


            modelBuilder.Entity<UserSecurity>(entity =>
            {
                entity.HasKey(x => x.UserId);

                entity.Property(x => x.IsConfirmed)
                    .HasDefaultValue(false);

                entity.HasMany(x => x.UserConfirmationTokens)
                    .WithOne(x => x.UserSecurity)
                    .HasForeignKey(x => x.UserId);

                entity.HasOne(e => e.User)
                    .WithOne(x => x.UserSecurity)
                    .HasForeignKey<UserSecurity>(x => x.UserId);
            });

            modelBuilder.Entity<UserConfirmationToken>(entity =>
            {
                entity.HasKey(x => x.ConfirmId);

                entity.HasOne(x => x.UserSecurity)
                    .WithMany(x => x.UserConfirmationTokens)
                    .HasForeignKey(x => x.UserId);

                entity.Property(x => x.ConfirmId).HasValueGenerator<GuidGenerator>();

                entity.Property(x => x.CreateDateTime).HasValueGenerator<CurrentDateTimeGenerator>();
            });


            modelBuilder.Entity<ReactionComment>(entity =>
            {
                entity.HasKey(e => new { e.ReactionId, e.UserId })
                    .HasName("ReactionComment_pk");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.ReactionComment)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReactionToComment_fk0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactionComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReactionComment_fk1");
            });

            modelBuilder.Entity<ReactionPost>(entity =>
            {
                entity.HasKey(e => new { e.ReactionId, e.UserId })
                    .HasName("ReactionPost_pk");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ReactionPost)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReactionToPost_fk0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactionPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReactionPost_fk1");
            });

            modelBuilder.Entity<ReactionTypeComment>(entity =>
            {
                entity.HasKey(e => e.ReactionId)
                    .HasName("ReactionTypeComment_pk");

                entity.Property(e => e.ReactionId).ValueGeneratedNever();

                entity.Property(e => e.Content).IsRequired();
            });

            modelBuilder.Entity<ReactionTypePost>(entity =>
            {
                entity.HasKey(e => e.ReactionId)
                    .HasName("ReactionTypePost_pk");

                entity.Property(e => e.ReactionId).ValueGeneratedNever();

                entity.Property(e => e.Content).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
