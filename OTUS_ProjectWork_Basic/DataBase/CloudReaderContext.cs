using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class CloudReaderContext : DbContext
{
    public CloudReaderContext()
    {
    }

    public CloudReaderContext(DbContextOptions<CloudReaderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usersticket> Userstickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=CloudReader;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор автора")
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата добавления автора в систему")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasComment("Полное имя автора")
                .HasColumnName("fullname");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasComment("Флаг активности автора (true = активен, false = скрыт из каталога)")
                .HasColumnName("isactive");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор книги")
                .HasColumnName("id");
            entity.Property(e => e.Authorid)
                .HasComment("Идентификатор автора книги")
                .HasColumnName("authorid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата добавления книги в систему")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasComment("Описание книги")
                .HasColumnName("description");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasComment("Флаг активности книги (true = доступна, false = недоступна)")
                .HasColumnName("isactive");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasComment("Цена книги")
                .HasColumnName("price");
            entity.Property(e => e.Publicationyear)
                .HasComment("Год публикации книги")
                .HasColumnName("publicationyear");
            entity.Property(e => e.Stockquantity)
                .HasDefaultValue(0)
                .HasComment("Количество экземпляров книги в наличии")
                .HasColumnName("stockquantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("Название книги")
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.Authorid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("books_authorid_fkey");

            entity.HasMany(d => d.Categories).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookscategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("Categoryid")
                        .HasConstraintName("bookscategories_categoryid_fkey"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("Bookid")
                        .HasConstraintName("bookscategories_bookid_fkey"),
                    j =>
                    {
                        j.HasKey("Bookid", "Categoryid").HasName("bookscategories_pkey");
                        j.ToTable("bookscategories");
                        j.IndexerProperty<int>("Bookid")
                            .HasComment("Идентификатор книги")
                            .HasColumnName("bookid");
                        j.IndexerProperty<int>("Categoryid")
                            .HasComment("Идентификатор категории")
                            .HasColumnName("categoryid");
                    });
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_pkey");

            entity.ToTable("cart");

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор записи в корзине")
                .HasColumnName("id");
            entity.Property(e => e.Bookid)
                .HasComment("Идентификатор книги")
                .HasColumnName("bookid");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasComment("Количество единиц книги в корзине")
                .HasColumnName("quantity");
            entity.Property(e => e.Userid)
                .HasComment("Идентификатор пользователя")
                .HasColumnName("userid");

            entity.HasOne(d => d.Book).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cart_bookid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cart_userid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор категории")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasComment("Описание категории")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("Название категории")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Accountname, "users_accountname_key").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор пользователя")
                .HasColumnName("id");
            entity.Property(e => e.Accountname)
                .HasMaxLength(200)
                .HasComment("Логин пользователя (уникальный)")
                .HasColumnName("accountname");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата регистрации пользователя")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasComment("Флаг активности пользователя (true = активен, false = заблокирован/удален)")
                .HasColumnName("isactive");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .HasComment("Имя пользователя (обязательное )")
                .HasColumnName("username");
        });

        modelBuilder.Entity<Usersticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usersticket_pkey");

            entity.ToTable("usersticket");

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор записи о покупке")
                .HasColumnName("id");
            entity.Property(e => e.Bookid)
                .HasComment("Идентификатор книги")
                .HasColumnName("bookid");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasComment("Итоговая цена покупки")
                .HasColumnName("price");
            entity.Property(e => e.Purchasedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата покупки")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("purchasedate");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasComment("Количество кинг")
                .HasColumnName("quantity");
            entity.Property(e => e.Userid)
                .HasComment("Идентификатор пользователя")
                .HasColumnName("userid");

            entity.HasOne(d => d.Book).WithMany(p => p.Userstickets)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("usersticket_bookid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Userstickets)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("usersticket_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
