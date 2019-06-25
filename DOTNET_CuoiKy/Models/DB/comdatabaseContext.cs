using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class comdatabaseContext : DbContext
    {
        public comdatabaseContext()
        {
        }

        public comdatabaseContext(DbContextOptions<comdatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Chitiethd> Chitiethd { get; set; }
        public virtual DbSet<Danhmuc> Danhmuc { get; set; }
        public virtual DbSet<Hoadon> Hoadon { get; set; }
        public virtual DbSet<Khachhang> Khachhang { get; set; }
        public virtual DbSet<Sanpham> Sanpham { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("host=db4free.net;port=3306;user=dotnetcuoiky;password=gVNUSduQf64Gncf;database=comdatabase;oldguids=true;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("admin", "comdatabase");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.HasKey(e => e.AutoId);

                entity.ToTable("Carts", "comdatabase");

                entity.HasIndex(e => e.SpId)
                    .HasName("cart_SpID_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("cart_UsrID_idx");

                entity.Property(e => e.AutoId)
                    .HasColumnName("autoID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SpId)
                    .HasColumnName("spID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Sp)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.SpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_SpID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_UsrID");
            });

            modelBuilder.Entity<Chitiethd>(entity =>
            {
                entity.HasKey(e => new { e.IdHd, e.IdSp });

                entity.ToTable("chitiethd", "comdatabase");

                entity.HasIndex(e => e.IdHd)
                    .HasName("hoadonFK_idx");

                entity.Property(e => e.IdHd)
                    .HasColumnName("idHD")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdSp)
                    .HasColumnName("idSp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GiaSp).HasColumnName("giaSP");

                entity.Property(e => e.Soluong)
                    .HasColumnName("soluong")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdHdNavigation)
                    .WithMany(p => p.Chitiethd)
                    .HasForeignKey(d => d.IdHd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("hoadonFK");

                entity.HasOne(d => d.IdSpNavigation)
                    .WithMany(p => p.Chitiethd)
                    .HasForeignKey(d => d.IdSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sanphamFK");
            });

            modelBuilder.Entity<Danhmuc>(entity =>
            {
                entity.HasKey(e => e.IddanhMuc);

                entity.ToTable("danhmuc", "comdatabase");

                entity.Property(e => e.IddanhMuc)
                    .HasColumnName("iddanhMuc")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TenDm)
                    .HasColumnName("tenDM")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Hoadon>(entity =>
            {
                entity.HasKey(e => e.Idhoadon);

                entity.ToTable("hoadon", "comdatabase");

                entity.HasIndex(e => e.IdNguoimua)
                    .HasName("nguoimuaID_idx");

                entity.Property(e => e.Idhoadon)
                    .HasColumnName("idhoadon")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdNguoimua)
                    .HasColumnName("idNguoimua")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdchiTiet)
                    .HasColumnName("idchiTiet")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NgayTao)
                    .HasColumnName("ngayTao")
                    .HasColumnType("date");

                entity.Property(e => e.SoLuong)
                    .HasColumnName("soLuong")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TinhTrang)
                    .HasColumnName("tinhTrang")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.TongTien).HasColumnName("tongTien");

                entity.HasOne(d => d.IdNguoimuaNavigation)
                    .WithMany(p => p.Hoadon)
                    .HasForeignKey(d => d.IdNguoimua)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("nguoimuaID");
            });

            modelBuilder.Entity<Khachhang>(entity =>
            {
                entity.HasKey(e => e.IdKhachHang);

                entity.ToTable("khachhang", "comdatabase");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdKhachHang)
                    .HasColumnName("idKhachHang")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NameKh)
                    .HasColumnName("nameKH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SoDiethoai)
                    .HasColumnName("soDiethoai")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Sanpham>(entity =>
            {
                entity.HasKey(e => e.IdsanPham);

                entity.ToTable("sanpham", "comdatabase");

                entity.HasIndex(e => e.DanhMuc)
                    .HasName("danhMucFK_idx");

                entity.Property(e => e.IdsanPham)
                    .HasColumnName("idsanPham")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DanhMuc)
                    .HasColumnName("danhMuc")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GiaSp).HasColumnName("giaSP");

                entity.Property(e => e.Hinh1)
                    .HasColumnName("hinh1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Hinh2)
                    .HasColumnName("hinh2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Hinh3)
                    .HasColumnName("hinh3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Hinh4)
                    .HasColumnName("hinh4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MoTa)
                    .HasColumnName("moTa")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TenSp)
                    .HasColumnName("tenSP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DanhMucNavigation)
                    .WithMany(p => p.Sanpham)
                    .HasForeignKey(d => d.DanhMuc)
                    .HasConstraintName("danhMucFK");
            });
        }
    }
}
