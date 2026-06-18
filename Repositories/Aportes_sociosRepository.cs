
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
    public class Aportes_sociosRepository : IAportes_sociosRepository
    {
        private readonly TDatosPostgreSQL _db;

        public Aportes_sociosRepository(TDatosPostgreSQL db)
        {
            _db = db;
        }

        private Aportes_sociosDto MapearAportes_socios(NpgsqlDataReader reader)
        {
            return new Aportes_sociosDto
                {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                SocioId = reader.GetInt32(reader.GetOrdinal("socio_id")),
                PeriodoId = reader.GetInt32(reader.GetOrdinal("periodo_id")),
                MontoSocio = reader.GetDecimal(reader.GetOrdinal("monto_socio")),
                MontoDeportivo = reader.GetDecimal(reader.GetOrdinal("monto_deportivo")),
                MontoGastos = reader.GetDecimal(reader.GetOrdinal("monto_gastos")),
                ConMulta = reader.GetBoolean(reader.GetOrdinal("con_multa")),
                MultaSocio = reader.GetDecimal(reader.GetOrdinal("multa_socio")),
                MultaDeportivo = reader.GetDecimal(reader.GetOrdinal("multa_deportivo")),
                TotalCobrado = reader.GetDecimal(reader.GetOrdinal("total_cobrado")),
                Estado = reader.GetString(reader.GetOrdinal("estado")),
                FechaPago = DateOnly.FromDateTime(
                    reader.GetDateTime(reader.GetOrdinal("fecha_pago"))
                ),
                RegistradoPor = reader.GetInt32(reader.GetOrdinal("registrado_por")),
                Observaciones = reader.GetString(reader.GetOrdinal("observaciones"))
                            };
        }

        public async Task<List<Aportes_sociosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_aportes_socios(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<Aportes_sociosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearAportes_socios(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<Aportes_sociosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_aportes_socios_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearAportes_socios(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(Aportes_sociosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_Insertar_aportes_socios(@p_socio_id, @p_periodo_id, @p_monto_socio, @p_monto_deportivo, @p_monto_gastos, @p_con_multa, @p_multa_socio, @p_multa_deportivo, @p_total_cobrado, @p_estado, @p_fecha_pago, @p_registrado_por, @p_observaciones);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_socio_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.SocioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_periodo_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.PeriodoId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_socio",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoSocio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_deportivo",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoDeportivo
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_gastos",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoGastos
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_con_multa",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.ConMulta
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_multa_socio",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MultaSocio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_multa_deportivo",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MultaDeportivo
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_total_cobrado",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.TotalCobrado
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_estado",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Estado
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_pago",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaPago
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_registrado_por",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.RegistradoPor
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_observaciones",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Observaciones
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

        public async Task<bool> ActualizarAsync(Aportes_sociosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_aportes_socios(@p_id, @p_socio_id, @p_periodo_id, @p_monto_socio, @p_monto_deportivo, @p_monto_gastos, @p_con_multa, @p_multa_socio, @p_multa_deportivo, @p_total_cobrado, @p_estado, @p_fecha_pago, @p_registrado_por, @p_observaciones);";
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
                    ParameterName = "@p_periodo_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.PeriodoId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_socio",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoSocio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_deportivo",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoDeportivo
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_monto_gastos",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MontoGastos
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_con_multa",
                    NpgsqlDbType = NpgsqlDbType.Boolean,
                    Value = entity.ConMulta
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_multa_socio",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MultaSocio
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_multa_deportivo",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.MultaDeportivo
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_total_cobrado",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Value = entity.TotalCobrado
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_estado",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Estado
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_fecha_pago",
                    NpgsqlDbType = NpgsqlDbType.Date,
                    Value = entity.FechaPago
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_registrado_por",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.RegistradoPor
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_observaciones",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Observaciones
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

                string sql = $"SELECT Fn_Eliminar_aportes_socios(@p_id);";
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
                string sql = $"SELECT fn_existe_aportes_socios(@p_criterio);";
                
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