using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: periodos
                /// Clave primaria: id
                /// Total columnas: 6
                /// </summary>
                [Table("periodos")]
                public partial class Periodos
                {
                [Column("id")]
    [Key]
    [Required]
    public int Id { get; set; }

    [Column("anio")]
    [Required]
    public int Anio { get; set; }

    [Column("mes")]
    [Required]
    public int Mes { get; set; }

    [Column("fecha_inicio")]
    [Required]
    public DateOnly Fecha_inicio { get; set; }

    [Column("fecha_cierre")]
    public DateOnly? Fecha_cierre { get; set; }

    [Column("dia_limite")]
    [Required]
    public int Dia_limite { get; set; }
                    }
               }