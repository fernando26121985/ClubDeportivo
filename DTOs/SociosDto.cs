using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para socios
                /// Data Transfer Object
                /// </summary>
                public class SociosDto
                {
                public int Id { get; set; }

                public string Nombre { get; set; }

                public string Apellido { get; set; }

                public DateOnly FechaIngreso { get; set; }

                public bool Activo { get; set; }
                }
            }