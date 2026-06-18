using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

    namespace ClubDeportivo.Models
    {
        /// <summary>
        /// Modelo generado automáticamente para la tabla: socios
        /// Clave primaria: id
        /// Total columnas: 5
        /// </summary>
        [Table("socios")]
        public partial class Socios
        {
            [Column("id")]
            [Key]
            [Required]
            public int Id { get; set; }

            [Column("nombre")]
            [Required]
            public string Nombre { get; set; }

            [Column("apellido")]
            [Required]
            public string Apellido { get; set; }

            [Column("fecha_ingreso")]
            [Required]
            public DateOnly Fecha_ingreso { get; set; }

            [Column("activo")]
            [Required]
            public bool Activo { get; set; }
        }
    }