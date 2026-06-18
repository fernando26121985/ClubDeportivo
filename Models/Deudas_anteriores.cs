using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: deudas_anteriores
                /// Clave primaria: id
                /// Total columnas: 7
                /// </summary>
                [Table("deudas_anteriores")]
                public partial class Deudas_anteriores
                {
                [Column("id")]
    [Key]
    [Required]
    public int Id { get; set; }

    [Column("socio_id")]
    [Required]
    public int Socio_id { get; set; }

    [Column("gestion")]
    [Required]
    public int Gestion { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("monto_original")]
    [Required]
    public decimal Monto_original { get; set; }

    [Column("saldo_pendiente")]
    [Required]
    public decimal Saldo_pendiente { get; set; }

    [Column("pagado")]
    [Required]
    public bool Pagado { get; set; }
                    }
               }