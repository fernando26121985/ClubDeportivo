using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para periodos
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IPeriodosRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(PeriodosDto periodosdto);
                  //  Task<int> InsertarRangeAsync(List<PeriodosDto> periodosdto);

                    // READ
                    Task<PeriodosDto> ObtenerPorIdAsync(int Id);
                   // Task<List<PeriodosDto>> ObtenerTodosAsync();
                    Task<List<PeriodosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<PeriodosDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(PeriodosDto periodosdto);
                   // Task<bool> ActualizarRangeAsync(List<PeriodosDto> periodosdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(PeriodosDto periodosdto);
                   // Task<bool> EliminarRangeAsync(List<PeriodosDto> periodosdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }