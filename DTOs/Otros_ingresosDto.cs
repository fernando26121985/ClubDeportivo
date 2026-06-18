using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para otros_ingresos
                /// Data Transfer Object
                /// </summary>
                public class Otros_ingresosDto
                {
                public int Id { get; set; }

    public int PeriodoId { get; set; }

    public string Concepto { get; set; }

    public decimal Monto { get; set; }

    public DateOnly Fecha { get; set; }

    public string Categoria { get; set; }

    public string? Referencia { get; set; }
                }
            }