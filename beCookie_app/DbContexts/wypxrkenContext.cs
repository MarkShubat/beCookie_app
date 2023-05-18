using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using beCookie_app.Models;

namespace beCookie_app.DbContexts
{
    public partial class wypxrkenContext : DbContext
    {
        public wypxrkenContext()
        {
        }

        public wypxrkenContext(DbContextOptions<wypxrkenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Avatar> Avatars { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Faq> Faqs { get; set; } = null!;
        public virtual DbSet<Like> Likes { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Point> Points { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=mouse.db.elephantsql.com;Port=5432;Database=wypxrken;Username=wypxrken;Password=zovwf0Mz-FrDy0wHWHiv-rPlY6dwvEXM");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("btree_gin")
                .HasPostgresExtension("btree_gist")
                .HasPostgresExtension("citext")
                .HasPostgresExtension("cube")
                .HasPostgresExtension("dblink")
                .HasPostgresExtension("dict_int")
                .HasPostgresExtension("dict_xsyn")
                .HasPostgresExtension("earthdistance")
                .HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("hstore")
                .HasPostgresExtension("intarray")
                .HasPostgresExtension("ltree")
                .HasPostgresExtension("pg_stat_statements")
                .HasPostgresExtension("pg_trgm")
                .HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("pgrowlocks")
                .HasPostgresExtension("pgstattuple")
                .HasPostgresExtension("tablefunc")
                .HasPostgresExtension("unaccent")
                .HasPostgresExtension("uuid-ossp")
                .HasPostgresExtension("xml2");

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.ToTable("avatars");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasMaxLength(25)
                    .HasColumnName("date");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Text)
                    .HasMaxLength(256)
                    .HasColumnName("text");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("comments_post_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("comments_user_id_fkey");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Adress)
                    .HasMaxLength(256)
                    .HasColumnName("adress");

                entity.Property(e => e.Date)
                    .HasMaxLength(25)
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.Location).HasColumnName("location");

                entity.Property(e => e.Schedule)
                    .HasMaxLength(100)
                    .HasColumnName("schedule");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasMaxLength(25)
                    .HasColumnName("type");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("events_admin_id_fkey");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("faq");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Text)
                    .HasMaxLength(1500)
                    .HasColumnName("text");

                entity.Property(e => e.Image_url)
                    .HasMaxLength(1000)
                    .HasColumnName("image_url");

                entity.Property(e => e.Tytle)
                    .HasMaxLength(100)
                    .HasColumnName("tytle");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("likes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("likes_post_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("likes_user_id_fkey");

                entity.Property(e => e.UserLiked).HasColumnName("userliked");
            });

            modelBuilder.Entity<Member>(entity =>
            {

                entity.ToTable("members");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("points");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adress)
                    .HasMaxLength(256)
                    .HasColumnName("adress");

                entity.Property(e => e.Date)
                    .HasMaxLength(25)
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.Location).HasColumnName("location");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Schedule)
                    .HasMaxLength(100)
                    .HasColumnName("schedule");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.Types)
                    .HasMaxLength(20)
                    .HasColumnName("types");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasMaxLength(25)
                    .HasColumnName("date");

                entity.Property(e => e.Text)
                    .HasMaxLength(256)
                    .HasColumnName("text");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("posts_user_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AvatarId).HasColumnName("avatar_id");
                entity.Property(e => e.Points).HasColumnName("points");
                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .HasColumnName("phone_number");

                entity.HasOne(d => d.Avatar)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AvatarId)
                    .HasConstraintName("users_avatar_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
