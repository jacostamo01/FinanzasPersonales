using Microsoft.EntityFrameworkCore;
using FinanzasPersonales.NET.Models;

namespace FinanzasPersonales.NET.Data
{
    public class FinanzasDbContext : DbContext
    {
        public FinanzasDbContext(DbContextOptions<FinanzasDbContext> options) : base(options)
        {
        }

        // entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Ahorro> Ahorros { get; set; }
        public DbSet<Inversion> Inversiones { get; set; }
        public DbSet<ColchonFinanciero> ColchonesFinancieros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Usuario 
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios"); 
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).HasColumnName("password").IsRequired().HasMaxLength(255);
                
                entity.Ignore(u => u.FechaCreacion);
                entity.HasIndex(u => u.Username).IsUnique();
            });

            //  Ingreso
            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.ToTable("ingresos");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.Monto).HasColumnName("monto").HasColumnType("double");
                entity.Property(i => i.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
                entity.Property(i => i.Fecha).HasColumnName("fecha").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(i => i.UsuarioId).HasColumnName("usuario_id");
            });

            //  Gasto
            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.ToTable("gastos");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Id).HasColumnName("id");
                entity.Property(g => g.Monto).HasColumnName("monto").HasColumnType("double");
                entity.Property(g => g.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
                entity.Property(g => g.Categoria).HasColumnName("categoria").HasMaxLength(100);
                entity.Property(g => g.Fecha).HasColumnName("fecha").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(g => g.UsuarioId).HasColumnName("usuario_id");
            });

            //  Ahorro
            modelBuilder.Entity<Ahorro>(entity =>
            {
                entity.ToTable("ahorros");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).HasColumnName("id");
                entity.Property(a => a.MontoObjetivo).HasColumnName("monto_objetivo").HasColumnType("double");
                entity.Property(a => a.MontoActual).HasColumnName("monto_actual").HasColumnType("double");
                entity.Property(a => a.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
                entity.Property(a => a.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(a => a.FechaObjetivo).HasColumnName("fecha_objetivo");
                entity.Property(a => a.UsuarioId).HasColumnName("usuario_id");
            });

            //  Inversion
            modelBuilder.Entity<Inversion>(entity =>
            {
                entity.ToTable("inversiones");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.MontoInicial).HasColumnName("monto_inicial").HasColumnType("double");
                entity.Property(i => i.ValorActual).HasColumnName("valor_actual").HasColumnType("double");
                entity.Property(i => i.TasaInteres).HasColumnName("tasa_interes").HasColumnType("double");
                entity.Property(i => i.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
                entity.Property(i => i.FechaInicio).HasColumnName("fecha_inicio").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(i => i.FechaVencimiento).HasColumnName("fecha_vencimiento");
                entity.Property(i => i.UsuarioId).HasColumnName("usuario_id");
            });

            // Colchon financiero
            modelBuilder.Entity<ColchonFinanciero>(entity =>
            {
                entity.ToTable("colchones_financieros");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id");
                entity.Property(c => c.UsuarioId).HasColumnName("usuario_id");
                entity.Property(c => c.MontoActual).HasColumnName("monto_actual").HasColumnType("double");
                entity.Property(c => c.Meta).HasColumnName("meta").HasColumnType("double");
                entity.Property(c => c.PorcentajeAhorro).HasColumnName("porcentaje_ahorro").HasColumnType("double");
                entity.Property(c => c.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
