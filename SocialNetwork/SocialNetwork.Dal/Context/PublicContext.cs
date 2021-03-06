﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ValueGenerators;

namespace SocialNetwork.Dal.Context
{
    public class PublicContext : DbContext
    {
        private readonly string _connectionString;

        public PublicContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PublicContext(DbContextOptions<PublicContext> options, string connectionString)
            : base(options)
        {
            _connectionString = connectionString;
        }

        internal IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Mention> Mention { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<PostTop> PostTop { get; set; }

        #region Security
        public virtual DbSet<UserSecurity> UserSecurity { get; set; }
        public virtual DbSet<UserConfirmationToken> UserConfirmationToken { get; set; }
        #endregion

        #region Reaction
        public virtual DbSet<ReactionComment> ReactionComment { get; set; }
        public virtual DbSet<ReactionPost> ReactionPost { get; set; }
        public virtual DbSet<ReactionTypeComment> ReactionTypeComment { get; set; }
        public virtual DbSet<ReactionTypePost> ReactionTypePost { get; set; }
        #endregion

        #region Board
        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<BoardType> BoardType { get; set; }
        #endregion

        #region Attachments
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<AttachmentComment> AttachmentComment { get; set; }
        public virtual DbSet<AttachmentPost> AttachmentPost { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence("post_comment_id_seq");

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Mention>(x => { x.HasKey(x => new {x.MentionId, x.MentionerId}); });

            modelBuilder.Entity<PostTop>(entity => { entity.HasKey(x => x.PostId); });

            modelBuilder.Entity<BoardSetting>(entity => { entity.HasKey(x => x.SettingId); });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CreateDateTime).HasValueGenerator<CurrentDateTimeGenerator>();

                entity.Property(e => e.ContentType).IsRequired();

                entity.Property(e => e.CreateDateTime).HasColumnType("date");

                entity.Property(e => e.Path).IsRequired();
            });

            modelBuilder.Entity<AttachmentComment>(entity =>
            {
                entity.HasKey(e => new { e.CommentId, e.AttachmentId })
                    .HasName("AttachmentComment_pkey");

            });

            modelBuilder.Entity<AttachmentPost>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.AttachmentId })
                    .HasName("AttachmentPost_pkey");


                entity.HasOne(d => d.Post)
                    .WithMany(p => p.AttachmentPost)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AttachmentPost_PostId_fkey");
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.BoardType)
                    .WithMany(p => p.Board)
                    .HasForeignKey(d => d.BoardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Board_BoardTypeId_fkey");

                entity.HasMany(x => x.BoardSettings)
                    .WithOne(x => x.Board)
                    .HasForeignKey(x => x.BoardId);
            });

            modelBuilder.Entity<BoardType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Date).HasValueGenerator<CurrentDateTimeGenerator>();

                entity.Property(e => e.Text).IsRequired();

                entity.HasMany(x => x.Mention)
                    .WithOne(x => x.Comment)
                    .HasForeignKey(x => x.MentionId);

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
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Date).HasValueGenerator<CurrentDateTimeGenerator>();

                entity.HasMany(x => x.Mention)
                    .WithOne(x => x.Post)
                    .HasForeignKey(x => x.MentionId);

                entity.HasOne(x => x.Board);

                entity.Property(e => e.Text).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Post_fk0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id).HasValueGenerator<GuidGenerator>();

                entity.HasOne(x => x.UserSecurity)
                    .WithOne(x => x.User);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(x => x.RoleId);
            });

            modelBuilder.Entity<UserSecurity>(entity =>
            {
                entity.HasKey(x => x.UserId);

                entity.HasOne(x => x.Role);

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
                entity.HasKey(e => new { e.UserId, e.CommentId })
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
                entity.HasKey(e => new { e.UserId, e.PostId })
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
        }
    }
}
