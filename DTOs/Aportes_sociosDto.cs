using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para aportes_socios
                /// Data Transfer Object
                /// </summary>
                public class Aportes_sociosDto
                {
                public int Id { get; set; }

    public int SocioId { get; set; }

    public int PeriodoId { get; set; }

    public decimal MontoSocio { get; set; }

    public decimal MontoDeportivo { get; set; }

    public decimal MontoGastos { get; set; }

    public bool ConMulta { get; set; }

    public decimal MultaSocio { get; set; }

    public decimal MultaDeportivo { get; set; }

    public decimal? TotalCobrado { get; set; }

    public string Estado { get; set; }

    public DateOnly? FechaPago { get; set; }

    public int? RegistradoPor { get; set; }

    public string? Observaciones { get; set; }
                }
            }