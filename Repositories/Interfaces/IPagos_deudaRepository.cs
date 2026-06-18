using System.Threading.Tasks;
            using System.Collections.Generic;
            using ClubDeportivo.Models;
            using ClubDeportivo.DTOs;

            namespace ClubDeportivo.Repositories.Interfaces
            {
                /// <summary>
                /// Interface generada para pagos_deuda
                /// Repository Pattern - CRUD completo
                /// </summary>
                public interface IPagos_deudaRepository
                {
                    // CREATE
                    Task<int> InsertarAsync(Pagos_deudaDto pagosdeudadto);
                  //  Task<int> InsertarRangeAsync(List<Pagos_deudaDto> pagosdeudadto);

                    // READ
                    Task<Pagos_deudaDto> ObtenerPorIdAsync(int Id);
                   // Task<List<Pagos_deudaDto>> ObtenerTodosAsync();
                    Task<List<Pagos_deudaDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
                  //  Task<List<Pagos_deudaDto>> BuscarAsync(string criterio);
                  //  Task<int> ContarAsync();
                    Task<bool> ExisteAsync(string criterio);

                    // UPDATE
                    Task<bool> ActualizarAsync(Pagos_deudaDto pagosdeudadto);
                   // Task<bool> ActualizarRangeAsync(List<Pagos_deudaDto> pagosdeudadto);

                    // DELETE
                    Task<bool> EliminarPorIdAsync(int Id);
                   // Task<bool> EliminarAsync(Pagos_deudaDto pagosdeudadto);
                   // Task<bool> EliminarRangeAsync(List<Pagos_deudaDto> pagosdeudadto);

                    // BATCH OPERATIONS
                   // Task<int> GuardarCambiosAsync();
                }
            }