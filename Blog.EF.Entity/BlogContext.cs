using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blog.EF.Entity
{
    public partial class BlogContext : DbContext
    {
        public BlogContext()
        {
        }

        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleCategories> ArticleCategories { get; set; }
        public virtual DbSet<ArticleTags> ArticleTags { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<OperationLog> OperationLog { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_Article_Title")
                    .IsUnique();

                entity.Property(e => e.Author).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Article_Users");
            });

            modelBuilder.Entity<ArticleCategories>(entity =>
            {
                entity.HasIndex(e => new { e.Article, e.Category });

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ArticleNavigation)
                    .WithMany(p => p.ArticleCategories)
                    .HasForeignKey(d => d.Article)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleCategories_Article");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.ArticleCategories)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleCategories_Category");
            });

            modelBuilder.Entity<ArticleTags>(entity =>
            {
                entity.HasIndex(e => new { e.Article, e.Tag });

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ArticleNavigation)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.Article)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleTags_Article");

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleTags_Tag");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Category")
                    .IsUnique();

                entity.HasIndex(e => new { e.Name, e.Id });

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OperationLog>(entity =>
            {
                entity.Property(e => e.Guid).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ipaddress).IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Role_Name_uindex")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Id });

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.UserRole_dbo.Role_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.UserRole_dbo.sysUserInfo_UserId");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.AuthUpdatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}