using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: egresos
                /// Clave primaria: id
                /// Total columnas: 6
                /// </summary>
                [Table("egresos")]
                public partial class Egresos
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
                    }
               }