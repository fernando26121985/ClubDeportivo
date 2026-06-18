using System.ComponentModel.DataAnnotations;

            namespace ClubDeportivo.DTOs
            {
                /// <summary>
                /// DTO generado para periodos
                /// Data Transfer Object
                /// </summary>
                public class PeriodosDto
                {
                public int Id { get; set; }

    public int Anio { get; set; }

    public int Mes { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public int DiaLimite { get; set; }
                }
            }