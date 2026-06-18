
using ClubDeportivo.TDatos;
using ClubDeportivo.DTOs;
using ClubDeportivo.Repositories.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubDeportivo.Repositories
{
    public class SociosRepository : ISociosRepository
    {
        private readonly TDatosPostgreSQL _db;

        public SociosRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private SociosDto MapearSocios(NpgsqlDataReader reader)
        {
            return new SociosDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                FechaIngreso = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha_ingreso"))
                ),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
        }

        public async Task<List<SociosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_socios(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<SociosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearSocios(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<SociosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_socios_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearSocios(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(SociosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_socios(@p_nombre, @p_apellido, @p_fecha_ingreso, @p_activo);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_nombre",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Nombre
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_apellido",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Apellido
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_ingreso",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaIngreso
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_activo",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.Activo
                });
                
                var resultado = await cmd.ExecuteScalarAsync();
                await _db.CommitTransactionAsync();
                return Convert.ToInt32(resultado);
            }
            catch
            {
                await _db.RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<bool> ActualizarAsync(SociosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_socios(@p_id, @p_nombre, @p_apellido, @p_fecha_ingreso, @p_activo);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_nombre",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Nombre
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_apellido",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Apellido
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_ingreso",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaIngreso
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_activo",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.Activo
                });
                
                var resultado = await cmd.ExecuteScalarAsync();
                await _db.CommitTransactionAsync();
                  return Convert.ToInt32(resultado) > 0;
            }
            catch
            {
                await _db.RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<bool> EliminarPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Eliminar_socios(@p_id);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                var resultado = await cmd.ExecuteScalarAsync();
                await _db.CommitTransactionAsync();
                return Convert.ToInt32(resultado) > 0;
            }
            catch
            {
                await _db.RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<bool> ExisteAsync(string Criterio)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT fn_existe_socios(@p_criterio);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@p_criterio", Value = Criterio });
                
                var resultado = await cmd.ExecuteScalarAsync();
                return Convert.ToBoolean(resultado);
            }
            catch
            {
                return false;
            }
        }

        // Métodos requeridos por interfaz
       
    }
}