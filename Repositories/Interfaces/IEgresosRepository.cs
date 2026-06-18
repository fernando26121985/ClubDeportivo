using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para egresos
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IEgresosRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(EgresosDto egresosdto);
                  //  Task<int> InsertarRangeAsync(List<EgresosDto> egresosdto);

                    // READ
                    Task<EgresosDto> ObtenerPorIdAsync(int Id);
                   // Task<List<EgresosDto>> ObtenerTodosAsync();
                    Task<List<EgresosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<EgresosDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(EgresosDto egresosdto);
                   // Task<bool> ActualizarRangeAsync(List<EgresosDto> egresosdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(EgresosDto egresosdto);
                   // Task<bool> EliminarRangeAsync(List<EgresosDto> egresosdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }