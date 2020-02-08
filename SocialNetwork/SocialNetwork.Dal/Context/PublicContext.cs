﻿using Microsoft.EntityFrameworkCore;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ValueGenerators;
using SocialNetwork.Utilities.Abstractions;

namespace SocialNetwork.Dal.Context
{
    public class PublicContext : DbContext
    {
        private readonly IConfigSettingService _configSettingService;

        public PublicContext(IConfigSettingService configSettingService)
        {
            _configSettingService = configSettingService;
        }

        public PublicContext(DbContextOptions<PublicContext> options, IConfigSettingService configSettingService)
            : base(options)
        {
            _configSettingService = configSettingService;
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        #region Security
        public virtual DbSet<UserSecurity> UserSecurity { get; set; }
        public virtual DbSet<UserConfirmationToken> UserConfirmationToken { get; set; }
        #endregion

        #region Reaction
        public virtual DbSet<ReactionComment> ReactionComment { get; set; }
        public virtual DbSet<ReactionPost> ReactionPost { get; set; }
        public virtual DbSet<Reaction> Reaction { get; set; }
        public virtual DbSet<ReactionTypeComment> ReactionTypeComment { get; set; }
        public virtual DbSet<ReactionTypePost> ReactionTypePost { get; set; }
        #endregion

        #region Group
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupPost> GroupPost { get; set; }
        public virtual DbSet<GroupType> GroupType { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configSettingService.GetSetting("connectionString", "default"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.Group)
                    .HasForeignKey(d => d.GroupTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Group_GroupTypeId_fkey");
            });

            modelBuilder.Entity<GroupPost>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.GroupId })
                    .HasName("GroupPost_pkey");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPost)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GroupPost_GroupId_fkey");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.GroupPost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GroupPost_PostId_fkey");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId })
                    .HasName("UserGroup_pkey");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserGroup_GroupId_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserGroup_UserId_fkey");
            });

            modelBuilder.Entity<GroupType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

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
