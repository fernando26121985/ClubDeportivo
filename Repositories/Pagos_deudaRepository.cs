
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
    public class Pagos_deudaRepository : IPagos_deudaRepository
    {
        private readonly TDatosPostgreSQL _db;

        public Pagos_deudaRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private Pagos_deudaDto MapearPagos_deuda(NpgsqlDataReader reader)
        {
            return new Pagos_deudaDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                DeudaId = reader.GetInt32(reader.GetOrdinal("deuda_id")),
                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                FechaPago = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha_pago"))
                ),
                Observacion = reader.GetString(reader.GetOrdinal("observacion"))
                            };
        }

        public async Task<List<Pagos_deudaDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_pagos_deuda(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<Pagos_deudaDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearPagos_deuda(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<Pagos_deudaDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_pagos_deuda_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearPagos_deuda(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(Pagos_deudaDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_pagos_deuda(@p_deuda_id, @p_usuario_id, @p_monto, @p_fecha_pago, @p_observacion);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_deuda_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.DeudaId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_usuario_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.UsuarioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.Monto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_pago",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaPago
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_observacion",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Observacion
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

        public async Task<bool> ActualizarAsync(Pagos_deudaDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_pagos_deuda(@p_id, @p_deuda_id, @p_usuario_id, @p_monto, @p_fecha_pago, @p_observacion);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_deuda_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.DeudaId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_usuario_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.UsuarioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.Monto
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_pago",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaPago
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_observacion",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Observacion
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

                string sql = $"SELECT Fn_Eliminar_pagos_deuda(@p_id);";
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
                string sql = $"SELECT fn_existe_pagos_deuda(@p_criterio);";
                
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