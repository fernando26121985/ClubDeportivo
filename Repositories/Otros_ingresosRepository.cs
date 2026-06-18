
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
    public class Otros_ingresosRepository : IOtros_ingresosRepository
    {
        private readonly TDatosPostgreSQL _db;

        public Otros_ingresosRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private Otros_ingresosDto MapearOtros_ingresos(NpgsqlDataReader reader)
        {
            return new Otros_ingresosDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PeriodoId = reader.GetInt32(reader.GetOrdinal("periodo_id")),
                Concepto = reader.GetString(reader.GetOrdinal("concepto")),
                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                Fecha = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha"))
                ),
                Categoria = reader.GetString(reader.GetOrdinal("categoria")),
                Referencia = reader.GetString(reader.GetOrdinal("referencia"))
                            };
        }

        public async Task<List<Otros_ingresosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_otros_ingresos(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<Otros_ingresosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearOtros_ingresos(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<Otros_ingresosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_otros_ingresos_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearOtros_ingresos(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(Otros_ingresosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_otros_ingresos(@p_periodo_id, @p_concepto, @p_monto, @p_fecha, @p_categoria, @p_referencia);";
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
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_referencia",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Referencia
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

        public async Task<bool> ActualizarAsync(Otros_ingresosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_otros_ingresos(@p_id, @p_periodo_id, @p_concepto, @p_monto, @p_fecha, @p_categoria, @p_referencia);";
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
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_referencia",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Referencia
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

                string sql = $"SELECT Fn_Eliminar_otros_ingresos(@p_id);";
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
                string sql = $"SELECT fn_existe_otros_ingresos(@p_criterio);";
                
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