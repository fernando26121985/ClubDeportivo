using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace ClubDeportivo.Models
            {
                /// <summary>
                /// Modelo generado automáticamente para la tabla: usuarios
                /// Clave primaria: id
                /// Total columnas: 6
                /// </summary>
                [Table("usuarios")]
                public partial class Usuarios
                {
                [Column("id")]
                [Key]
                [Required]
                public int Id { get; set; }

                [Column("socio_id")]
                public int? Socio_id { get; set; }

                [Column("username")]
                [Required]
                public string Username { get; set; }

                [Column("password_hash")]
                [Required]
                public string Password_hash { get; set; }

                [Column("rol")]
                [Required]
                public string Rol { get; set; }

                [Column("activo")]
                [Required]
                public bool Activo { get; set; }
                    }
               }