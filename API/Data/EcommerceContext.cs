using Microsoft.EntityFrameworkCore;
using API.Models;
namespace API.Data;
public partial class EcommerceContext : DbContext
{
    public EcommerceContext() {}
    public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) {}
    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27D960A4EE");
            entity.HasIndex(e => e.Mail, "UQ__Users__2724B2D1DF35970C").IsUnique();
            entity.HasIndex(e => e.Number, "UQ__Users__78A1A19DA9AD604A").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.Mail).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Number).HasMaxLength(10).IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(50).IsUnicode(false);
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}