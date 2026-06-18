using System.Collections.Generic;

            namespace MiSistema.DTOs
            {
                /// <summary>
                /// Clase para resultados paginados
                /// </summary>
                public class PagedResult<T>
                {
                    public List<T> Items { get; set; } = new();
                    public int TotalRegistros { get; set; }
                    public int PaginaActual { get; set; }
                    public int TotalPaginas { get; set; }
                    public int RegistrosPorPagina { get; set; }
                }
            }