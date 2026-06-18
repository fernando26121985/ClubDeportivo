using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para otros_ingresos
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IOtros_ingresosRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(Otros_ingresosDto otrosingresosdto);
                  //  Task<int> InsertarRangeAsync(List<Otros_ingresosDto> otrosingresosdto);

                    // READ
                    Task<Otros_ingresosDto> ObtenerPorIdAsync(int Id);
                   // Task<List<Otros_ingresosDto>> ObtenerTodosAsync();
                    Task<List<Otros_ingresosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<Otros_ingresosDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(Otros_ingresosDto otrosingresosdto);
                   // Task<bool> ActualizarRangeAsync(List<Otros_ingresosDto> otrosingresosdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(Otros_ingresosDto otrosingresosdto);
                   // Task<bool> EliminarRangeAsync(List<Otros_ingresosDto> otrosingresosdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }