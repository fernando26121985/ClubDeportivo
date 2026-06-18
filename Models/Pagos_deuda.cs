using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: pagos_deuda
                /// Clave primaria: id
                /// Total columnas: 6
                /// </summary>
                [Table("pagos_deuda")]
                public partial class Pagos_deuda
                {
                [Column("id")]
    [Key]
    [Required]
    public int Id { get; set; }

    [Column("deuda_id")]
    [Required]
    public int Deuda_id { get; set; }

    [Column("usuario_id")]
    public int? Usuario_id { get; set; }

    [Column("monto")]
    [Required]
    public decimal Monto { get; set; }

    [Column("fecha_pago")]
    [Required]
    public DateOnly Fecha_pago { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }
                    }
               }