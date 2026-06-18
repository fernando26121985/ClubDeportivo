
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
    public class EgresosRepository : IEgresosRepository
    {
        private readonly TDatosPostgreSQL _db;

        public EgresosRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private EgresosDto MapearEgresos(NpgsqlDataReader reader)
        {
            return new EgresosDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PeriodoId = reader.GetInt32(reader.GetOrdinal("periodo_id")),
                Concepto = reader.GetString(reader.GetOrdinal("concepto")),
                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                Fecha = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha"))
                ),
                Categoria = reader.GetString(reader.GetOrdinal("categoria"))
                            };
        }

        public async Task<List<EgresosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_egresos(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<EgresosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearEgresos(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<EgresosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_egresos_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearEgresos(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(EgresosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_egresos(@p_periodo_id, @p_concepto, @p_monto, @p_fecha, @p_categoria);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_periodo_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.PeriodoId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_concepto",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Concepto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.Monto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.Fecha
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_categoria",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Categoria
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

        public async Task<bool> ActualizarAsync(EgresosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_egresos(@p_id, @p_periodo_id, @p_concepto, @p_monto, @p_fecha, @p_categoria);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_periodo_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.PeriodoId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_concepto",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Concepto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.Monto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.Fecha
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_categoria",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Categoria
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

                string sql = $"SELECT Fn_Eliminar_egresos(@p_id);";
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
                string sql = $"SELECT fn_existe_egresos(@p_criterio);";
                
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