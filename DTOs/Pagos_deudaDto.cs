using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para pagos_deuda
                /// Data Transfer Object
                /// </summary>
                public class Pagos_deudaDto
                {
                public int Id { get; set; }

    public int DeudaId { get; set; }

    public int? UsuarioId { get; set; }

    public decimal Monto { get; set; }

    public DateOnly FechaPago { get; set; }

    public string? Observacion { get; set; }
                }
            }