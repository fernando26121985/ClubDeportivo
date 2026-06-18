using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para deudas_anteriores
                /// Data Transfer Object
                /// </summary>
                public class Deudas_anterioresDto
                {
                public int Id { get; set; }

    public int SocioId { get; set; }

    public int Gestion { get; set; }

    public string? Descripcion { get; set; }

    public decimal MontoOriginal { get; set; }

    public decimal SaldoPendiente { get; set; }

    public bool Pagado { get; set; }
                }
            }