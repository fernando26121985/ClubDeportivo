
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
    public class Deudas_anterioresRepository : IDeudas_anterioresRepository
    {
        private readonly TDatosPostgreSQL _db;

        public Deudas_anterioresRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private Deudas_anterioresDto MapearDeudas_anteriores(NpgsqlDataReader reader)
        {
            return new Deudas_anterioresDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                SocioId = reader.GetInt32(reader.GetOrdinal("socio_id")),
                Gestion = reader.GetInt32(reader.GetOrdinal("gestion")),
                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                MontoOriginal = reader.GetDecimal(reader.GetOrdinal("monto_original")),
                SaldoPendiente = reader.GetDecimal(reader.GetOrdinal("saldo_pendiente")),
                Pagado = reader.GetBoolean(reader.GetOrdinal("pagado"))
                            };
        }

        public async Task<List<Deudas_anterioresDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_deudas_anteriores(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<Deudas_anterioresDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearDeudas_anteriores(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<Deudas_anterioresDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_deudas_anteriores_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearDeudas_anteriores(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(Deudas_anterioresDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_deudas_anteriores(@p_socio_id, @p_gestion, @p_descripcion, @p_monto_original, @p_saldo_pendiente, @p_pagado);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_socio_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.SocioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_gestion",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Gestion
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_descripcion",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Descripcion
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_original",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoOriginal
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_saldo_pendiente",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.SaldoPendiente
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_pagado",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.Pagado
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

        public async Task<bool> ActualizarAsync(Deudas_anterioresDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_deudas_anteriores(@p_id, @p_socio_id, @p_gestion, @p_descripcion, @p_monto_original, @p_saldo_pendiente, @p_pagado);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_socio_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.SocioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_gestion",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Gestion
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_descripcion",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Descripcion
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_original",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoOriginal
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_saldo_pendiente",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.SaldoPendiente
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_pagado",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.Pagado
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

                string sql = $"SELECT Fn_Eliminar_deudas_anteriores(@p_id);";
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
                string sql = $"SELECT fn_existe_deudas_anteriores(@p_criterio);";
                
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