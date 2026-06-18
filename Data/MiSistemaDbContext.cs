using ClubDeportivo.Models;
using Microsoft.EntityFrameworkCore;


        namespace ClubDeportivo.Data
        {
            /// <summary>
            /// DbContext generado automáticamente
            /// Tablas incluidas: socios, usuarios, aportes_socios, periodos, deudas_anteriores, pagos_deuda, otros_ingresos, egresos
            /// </summary>
            public partial class MiSistemaDbContext : DbContext
            {
                public MiSistemaDbContext(DbContextOptions<MiSistemaDbContext> options) : base(options) { }

               public DbSet<Socios> Socios { get; set; }
                public DbSet<Usuarios> Usuarios { get; set; }
                public DbSet<Aportes_socios> Aportes_socios { get; set; }
                public DbSet<Periodos> Periodos { get; set; }
                public DbSet<Deudas_anteriores> Deudas_anteriores { get; set; }
                public DbSet<Pagos_deuda> Pagos_deuda { get; set; }
                public DbSet<Otros_ingresos> Otros_ingresos { get; set; }
                public DbSet<Egresos> Egresos { get; set; }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder);
                    // Configuraciones adicionales aquí
                }
            }
        }