
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
    public class PeriodosRepository : IPeriodosRepository
    {
        private readonly TDatosPostgreSQL _db;

        public PeriodosRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private PeriodosDto MapearPeriodos(NpgsqlDataReader reader)
        {
            return new PeriodosDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Anio = reader.GetInt32(reader.GetOrdinal("anio")),
                Mes = reader.GetInt32(reader.GetOrdinal("mes")),
                FechaInicio = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha_inicio"))
                ),
                FechaCierre = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha_cierre"))
                ),
                DiaLimite = reader.GetInt32(reader.GetOrdinal("dia_limite"))
                            };
        }

        public async Task<List<PeriodosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_periodos(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<PeriodosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearPeriodos(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<PeriodosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_periodos_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearPeriodos(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(PeriodosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_periodos(@p_anio, @p_mes, @p_fecha_inicio, @p_fecha_cierre, @p_dia_limite);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_anio",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Anio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_mes",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Mes
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_inicio",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaInicio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_cierre",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaCierre
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_dia_limite",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.DiaLimite
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

        public async Task<bool> ActualizarAsync(PeriodosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_periodos(@p_id, @p_anio, @p_mes, @p_fecha_inicio, @p_fecha_cierre, @p_dia_limite);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_anio",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Anio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_mes",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Mes
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_inicio",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaInicio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_cierre",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaCierre
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_dia_limite",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.DiaLimite
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

                string sql = $"SELECT Fn_Eliminar_periodos(@p_id);";
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
                string sql = $"SELECT fn_existe_periodos(@p_criterio);";
                
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