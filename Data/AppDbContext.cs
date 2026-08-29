using Microsoft.EntityFrameworkCore;
using ASP_Library.Entities;

namespace ASP_Library.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // authors table
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("authors");
            entity.HasKey(e => e.AuthorId);
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();
        });

        // books table
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("books");
            entity.HasKey(e => e.BookId);
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Isbn)
                .HasColumnName("isbn")
                .HasMaxLength(13)
                .IsRequired();
            entity.HasIndex(e => e.Isbn).IsUnique();
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();
            entity.Property(e => e.PublishDate)
                .HasColumnName("publish_date")
                .HasColumnType("DATE");
        });

        // book_authors table
        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.ToTable("book_authors");
            entity.HasKey(e => new { e.BookId, e.AuthorId });
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasMaxLength(50);

            entity.HasOne(e => e.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
