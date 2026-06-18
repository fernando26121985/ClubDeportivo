using System.Threading.Tasks;
using System.Collections.Generic;
using ClubDeportivo.Models;
using ClubDeportivo.DTOs;
using ClubDeportivo.DTOs.Usu;

namespace ClubDeportivo.Repositories.Interfaces
            {
    /// <summary>
    /// Interface generada para usuarios
    /// Repository Pattern - CRUD completo
    /// </summary>
    public interface IUsuariosRepository
    {
        // CREATE
        Task<int> InsertarAsync(UsuariosDto usuariosdto);
        // Task<int> InsertarRangeAsync(List<UsuariosDto> usuariosdto);

        // READ
        Task<UsuariosDto> ObtenerPorIdAsync(int Id);
        // Task<List<UsuariosDto>> ObtenerTodosAsync();
        Task<List<UsuariosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10);
        // Task<List<UsuariosDto>> BuscarAsync(string criterio);
        // Task<int> ContarAsync();
        Task<bool> ExisteAsync(string criterio);

        // UPDATE
        Task<bool> ActualizarAsync(UsuariosDto usuariosdto);
        // Task<bool> ActualizarRangeAsync(List<UsuariosDto> usuariosdto);

        // DELETE
        Task<bool> EliminarPorIdAsync(int Id);
        // Task<bool> EliminarAsync(UsuariosDto usuariosdto);
        // Task<bool> EliminarRangeAsync(List<UsuariosDto> usuariosdto);

        Task<UsuarioLoginRespuestaDto> LoginAsync(UsuarioLoginDto usuarioLoginDto); 
     

        // BATCH OPERATIONS
        // Task<int> GuardarCambiosAsync();
    }
            }