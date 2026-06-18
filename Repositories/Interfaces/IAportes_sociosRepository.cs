using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para aportes_socios
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IAportes_sociosRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(Aportes_sociosDto aportessociosdto);
                  //  Task<int> InsertarRangeAsync(List<Aportes_sociosDto> aportessociosdto);

                    // READ
                    Task<Aportes_sociosDto> ObtenerPorIdAsync(int Id);
                   // Task<List<Aportes_sociosDto>> ObtenerTodosAsync();
                    Task<List<Aportes_sociosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<Aportes_sociosDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(Aportes_sociosDto aportessociosdto);
                   // Task<bool> ActualizarRangeAsync(List<Aportes_sociosDto> aportessociosdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(Aportes_sociosDto aportessociosdto);
                   // Task<bool> EliminarRangeAsync(List<Aportes_sociosDto> aportessociosdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }