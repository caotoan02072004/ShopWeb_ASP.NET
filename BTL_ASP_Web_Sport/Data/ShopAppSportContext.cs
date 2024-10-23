using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTL_ASP_Web_Sport.Data;

public partial class ShopAppSportContext : DbContext
{
    public ShopAppSportContext()
    {
    }

    public ShopAppSportContext(DbContextOptions<ShopAppSportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ShopAppSport;Persist Security Info=True;User ID=sa;Password=1234$;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Accounts__CB9A1CFF9A407B54");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserActive)
                .HasDefaultValue(1)
                .HasColumnName("userActive");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userAddress");
            entity.Property(e => e.UserAvatar)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userAvatar");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userEmail");
            entity.Property(e => e.UserFullName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userFullName");
            entity.Property(e => e.UserGender)
                .HasDefaultValue(1)
                .HasColumnName("userGender");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userPassword");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userPhone");
            entity.Property(e => e.UserRole)
                .HasDefaultValue(0)
                .HasColumnName("userRole");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__FA0AA72DA3ADF315");

            entity.Property(e => e.BlogId).HasColumnName("blogId");
            entity.Property(e => e.BlogDescription)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("ntext")
                .HasColumnName("blogDescription");
            entity.Property(e => e.BlogImage)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("blogImage");
            entity.Property(e => e.BlogName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("blogName");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__415B03B8456EC0BC");

            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("quantity");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("totalAmount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Carts__productId__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Carts__userId__5629CD9C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Categori__A88B4DE47F9A3BAD");

            entity.Property(e => e.CateId).HasColumnName("cateId");
            entity.Property(e => e.CateName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("cateName");
            entity.Property(e => e.Status).HasDefaultValueSql("(NULL)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809335D021F1A2E");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.OrderAddress)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderAddress");
            entity.Property(e => e.OrderAmount)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderAmount");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.OrderEmail)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderEmail");
            entity.Property(e => e.OrderFullName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderFullName");
            entity.Property(e => e.OrderNote)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("ntext")
                .HasColumnName("orderNote");
            entity.Property(e => e.OrderPaymentMethods)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderPaymentMethods");
            entity.Property(e => e.OrderPhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderPhone");
            entity.Property(e => e.OrderStatus)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderStatus");
            entity.Property(e => e.OrderStatusPayment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderStatusPayment");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__userId__628FA481");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE4AA83705A9");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasDefaultValue(0.0)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.TotalMoney)
                .HasDefaultValue(0.0)
                .HasColumnName("totalMoney");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__68487DD7");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__693CA210");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Products__5BBBEEF5772E1568");

            entity.Property(e => e.ProId).HasColumnName("proId");
            entity.Property(e => e.ProCategoryId).HasColumnName("proCategoryId");
            entity.Property(e => e.ProDescription)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("ntext")
                .HasColumnName("proDescription");
            entity.Property(e => e.ProImage)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("proImage");
            entity.Property(e => e.ProName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("proName");
            entity.Property(e => e.ProPrice)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("proPrice");
            entity.Property(e => e.ProSalePrice)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("proSalePrice");
            entity.Property(e => e.ProStatus)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("proStatus");

            entity.HasOne(d => d.ProCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProCategoryId)
                .HasConstraintName("FK__Products__proCat__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
