using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: aportes_socios
                /// Clave primaria: id
                /// Total columnas: 14
                /// </summary>
                [Table("aportes_socios")]
                public partial class Aportes_socios
                {
                [Column("id")]
    [Key]
    [Required]
    public int Id { get; set; }

    [Column("socio_id")]
    [Required]
    public int Socio_id { get; set; }

    [Column("periodo_id")]
    [Required]
    public int Periodo_id { get; set; }

    [Column("monto_socio")]
    [Required]
    public decimal Monto_socio { get; set; }

    [Column("monto_deportivo")]
    [Required]
    public decimal Monto_deportivo { get; set; }

    [Column("monto_gastos")]
    [Required]
    public decimal Monto_gastos { get; set; }

    [Column("con_multa")]
    [Required]
    public bool Con_multa { get; set; }

    [Column("multa_socio")]
    [Required]
    public decimal Multa_socio { get; set; }

    [Column("multa_deportivo")]
    [Required]
    public decimal Multa_deportivo { get; set; }

    [Column("total_cobrado")]
    public decimal? Total_cobrado { get; set; }

    [Column("estado")]
    [Required]
    public string Estado { get; set; }

    [Column("fecha_pago")]
    public DateOnly? Fecha_pago { get; set; }

    [Column("registrado_por")]
    public int? Registrado_por { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }
                    }
               }