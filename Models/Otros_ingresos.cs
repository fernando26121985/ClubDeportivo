using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: otros_ingresos
                /// Clave primaria: id
                /// Total columnas: 7
                /// </summary>
                [Table("otros_ingresos")]
                public partial class Otros_ingresos
                {
                [Column("id")]
    [Key]
    [Required]
    public int Id { get; set; }

    [Column("periodo_id")]
    [Required]
    public int Periodo_id { get; set; }

    [Column("concepto")]
    [Required]
    public string Concepto { get; set; }

    [Column("monto")]
    [Required]
    public decimal Monto { get; set; }

    [Column("fecha")]
    [Required]
    public DateOnly Fecha { get; set; }

    [Column("categoria")]
    [Required]
    public string Categoria { get; set; }

    [Column("referencia")]
    public string? Referencia { get; set; }
                    }
               }