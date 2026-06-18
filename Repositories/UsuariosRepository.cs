
using ClubDeportivo.TDatos;
using ClubDeportivo.DTOs;
using ClubDeportivo.Repositories.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubDeportivo.DTOs.Usu;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;
using ClubDeportivo.Models;
using System.Data;

namespace ClubDeportivo.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly TDatosPostgreSQL _db;
        private string claveSecreta;

        public UsuariosRepository(TDatosPostgreSQL db, IConfiguration config)
        {
            _db = db;

            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        private UsuariosDto MapearUsuarios(NpgsqlDataReader reader)
        {
            return new UsuariosDto
                {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                SocioId = reader.GetInt32(reader.GetOrdinal("socio_id")),
                Email = reader.GetString(reader.GetOrdinal("username")),
                Clave = reader.GetString(reader.GetOrdinal("password_hash")),
                Rol = reader.GetString(reader.GetOrdinal("rol")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
        }
        // Mapper específico para login (sin password_hash)
        private UsuariosDto MapearUsuarioLogin(NpgsqlDataReader reader)
        {
            return new UsuariosDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                SocioId = reader.IsDBNull(reader.GetOrdinal("socio_id"))
                                   ? null
                                   : reader.GetInt32(reader.GetOrdinal("socio_id")),
                Email = reader.GetString(reader.GetOrdinal("username")),
                Rol = reader.GetString(reader.GetOrdinal("rol")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                Clave = string.Empty  // ← nunca devolver la clave real
            };
        }
        public async Task<List<UsuariosDto>> ObtenerPaginadoAsync(int pagina = 1, int filas = 10)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = "SELECT * FROM fn_listar_usuarios(@pagina, @filas);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@pagina", Value = pagina });
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = "@filas", Value = filas });
                
                using var reader = await cmd.ExecuteReaderAsync();
                var lista = new List<UsuariosDto>();
                while (await reader.ReadAsync())
                    lista.Add(MapearUsuarios(reader));
                return lista;
            }
            finally
            {
                await _db.CloseConnectionAsync();
            }
        }

        public async Task<UsuariosDto> ObtenerPorIdAsync(int Id)
        {
            try
            {
                await _db.OpenConnectionAsync();
                string sql = $"SELECT * FROM fn_obtener_usuarios_porid(@p_id);";
                
                using var cmd = new NpgsqlCommand(sql, _db.Connection);
                cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = $"@p_id", Value = Id });
                
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    return MapearUsuarios(reader);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> InsertarAsync(UsuariosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                var passwordEncriptado = obtenermd5(entity.Clave);
                string sql = $"SELECT Fn_Insertar_usuarios(@p_socio_id, @p_username, @p_password_hash, @p_rol, @p_activo);";
                using var cmd = new NpgsqlCommand(sql, _db.Connection, _db.Transaction);
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_socio_id",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Value = entity.SocioId
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_username",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Email
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_password_hash",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = passwordEncriptado
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_rol",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Rol
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

        public async Task<bool> ActualizarAsync(UsuariosDto entity)
        {
            try
            {
                await _db.OpenConnectionAsync();
                await _db.BeginTransactionAsync();

                string sql = $"SELECT Fn_actualizar_usuarios(@p_id, @p_socio_id, @p_username, @p_password_hash, @p_rol, @p_activo);";
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
                    ParameterName = "@p_username",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Email
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_password_hash",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Clave
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@p_rol",
                    NpgsqlDbType = NpgsqlDbType.Text,
                    Value = entity.Rol
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

                string sql = $"SELECT Fn_Eliminar_usuarios(@p_id);";
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
                string sql = $"SELECT fn_existe_usuarios(@p_criterio);";
                
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

        public async Task<UsuarioLoginRespuestaDto> LoginAsync(UsuarioLoginDto usuarioLoginDto)
        {
           

            // Validación básica
            if (string.IsNullOrWhiteSpace(usuarioLoginDto.Email) || string.IsNullOrWhiteSpace(usuarioLoginDto.Clave))
                throw new ArgumentException("Usuario y clave son obligatorios.");

            var passwordEncriptado = obtenermd5(usuarioLoginDto.Clave);

            try
            {
                await _db.OpenConnectionAsync();
                var connection = _db.Connection;
                // Ejecutar SP para validar credenciales
                string sql = "SELECT * FROM sp_login_usuario(@p_usuario, @p_clave)";

                using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@p_usuario", usuarioLoginDto.Email);
                cmd.Parameters.AddWithValue("@p_clave", passwordEncriptado);
                using var reader = await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    return null; // credenciales incorrectas → devuelve null sin excepción

                var usuarioDto = MapearUsuarioLogin(reader);
                await reader.CloseAsync();
                // Generar token (puede ser JWT, aquí usamos GUID como ejemplo)
                // otro ejejmplo var tokenGenerado = Guid.NewGuid().ToString();
                var manejadorToken = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(claveSecreta);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuarioDto.Email.ToString()),
                        new Claim(ClaimTypes.Role, usuarioDto.Rol),
                        new Claim("id",            usuarioDto.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = manejadorToken.CreateToken(tokenDescriptor);
                // Retornar DTO de respuesta
                return new UsuarioLoginRespuestaDto
                {
                    Token = manejadorToken.WriteToken(token),
                    Usuario = usuarioDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en login: {ex.Message}");
                throw;
            }
            finally
            {
                // ✅ CORRECCIÓN 9: siempre cerrar la conexión en el finally
                // igual que haces en ObtenerPorIdAsync
                await _db.CloseConnectionAsync();
            }
        }




        // Métodos requeridos por interfaz
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}