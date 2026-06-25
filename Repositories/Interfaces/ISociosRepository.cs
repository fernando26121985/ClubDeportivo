using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para socios
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface ISociosRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(SociosDto sociosdto);
                  //  Task<int> InsertarRangeAsync(List<SociosDto> sociosdto);

                    // READ
                    Task<SociosDto> ObtenerPorIdAsync(int Id);
                    Task<List<SociosDto>> ObtenerTodosAsync();
                    Task<List<SociosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<SociosDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(SociosDto sociosdto);
                   // Task<bool> ActualizarRangeAsync(List<SociosDto> sociosdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(SociosDto sociosdto);
                   // Task<bool> EliminarRangeAsync(List<SociosDto> sociosdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }