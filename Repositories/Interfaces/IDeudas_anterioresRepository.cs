using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para deudas_anteriores
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IDeudas_anterioresRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(Deudas_anterioresDto deudasanterioresdto);
                  //  Task<int> InsertarRangeAsync(List<Deudas_anterioresDto> deudasanterioresdto);

                    // READ
                    Task<Deudas_anterioresDto> ObtenerPorIdAsync(int Id);
                   // Task<List<Deudas_anterioresDto>> ObtenerTodosAsync();
                    Task<List<Deudas_anterioresDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<Deudas_anterioresDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(Deudas_anterioresDto deudasanterioresdto);
                   // Task<bool> ActualizarRangeAsync(List<Deudas_anterioresDto> deudasanterioresdto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(Deudas_anterioresDto deudasanterioresdto);
                   // Task<bool> EliminarRangeAsync(List<Deudas_anterioresDto> deudasanterioresdto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }