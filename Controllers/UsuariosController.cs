using ClubDeportivo.DTOs;
using ClubDeportivo.DTOs.Usu;
using ClubDeportivo.Repositories.Interfaces;
                
                using Microsoft.AspNetCore.Authorization;
                using Microsoft.AspNetCore.Mvc;

                namespace ClubDeportivo.Controller
                {
                    [Route("api/usuarios")]
                    [ApiController]
                    public class UsuariosController : ControllerBase
                    {
                        private readonly IUsuariosRepository _usuariosRepo;
                
                        private readonly ILogger<UsuariosController> _logger;

                        public UsuariosController(
                            IUsuariosRepository usuariosRepo,
                          
                            ILogger<UsuariosController> logger)
                        {
                            _usuariosRepo = usuariosRepo;
                          
                            _logger = logger;
                        }
                        [AllowAnonymous]
                        [HttpPost("login")]
                        [ProducesResponseType(typeof(UsuarioLoginRespuestaDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto loginDto)
                        {
                            // ================================
                            // VALIDAR MODELO
                            // ================================
                            if (!ModelState.IsValid)
                                return BadRequest(ModelState);

                            if (string.IsNullOrWhiteSpace(loginDto.Email) ||
                                string.IsNullOrWhiteSpace(loginDto.Clave))
                                return BadRequest("Usuario y contraseña son obligatorios.");

                            try
                            {
                                // ================================
                                // LLAMAR AL REPOSITORIO
                                // ================================
                                var respuesta = await _usuariosRepo.LoginAsync(loginDto);

                                // ================================
                                // CREDENCIALES INCORRECTAS
                                // ================================
                                if (respuesta == null)
                                {
                                    _logger.LogWarning(
                                        "Intento de login fallido para usuario: {usuario}",
                                        loginDto.Email);

                                    // 401 sin revelar si el usuario existe o no
                                    return Unauthorized(new
                                    {
                                        mensaje = "Usuario o contraseña incorrectos."
                                    });
                                }

                                // ================================
                                // LOGIN EXITOSO
                                // ================================
                                _logger.LogInformation(
                                    "Login exitoso para usuario: {usuario} - Rol: {rol}",
                                    respuesta.Usuario.Email,
                                    respuesta.Usuario.Rol);

                                return Ok(new
                                {
                                    mensaje = "Login exitoso",
                                    token = respuesta.Token,
                                    usuario = new
                                    {
                                        respuesta.Usuario.Id,
                                        respuesta.Usuario.Email,
                                        respuesta.Usuario.Rol,
                                        respuesta.Usuario.SocioId
                                        // ✅ NO devolver PasswordHash al cliente nunca
                                    }
                                });
                                }
                                catch (ArgumentException ex)
                                {
                                    // Validaciones del repositorio
                                    return BadRequest(new { mensaje = ex.Message });
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error en login usuario: {usuario}", 
                                        loginDto.Email);
                                    return StatusCode(500, "Error interno del servidor.");
                                }
                            }

                        // =========================================
                        // INSERTAR
                        // =========================================
                       // [Authorize]
                        [HttpPost("insertar")]
                        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> InsertarUsuarios(
                            [FromBody] UsuariosDto dto)
                        {
                            try
                            {
                                // ================================
                                // VALIDAR MODELO
                                // ================================
                                if (!ModelState.IsValid)
                                {
                                    return BadRequest(ModelState);
                                }

                                // ================================
                                // INSERTAR
                                // ================================
                                var idInsertado =
                                    await _usuariosRepo.InsertarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (idInsertado <= 0)
                                {
                                    _logger.LogWarning(
                                        "No se pudo insertar la Usuarios.");

                                    return StatusCode(
                                        500,
                                        "No se pudo insertar la Usuarios.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Usuarios insertada correctamente. ID: {id}",
                                    idInsertado);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Usuarios insertada correctamente",
                                    id = idInsertado
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al insertar Usuarios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // MODIFICAR
                        // =========================================
                       // [Authorize]
                        [HttpPut("actualizar/{id}")]
                        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> ActualizarUsuarios(int id, [FromBody] UsuariosDto dto)
                        {
                            try
                            {
                                // ================================
                                // VALIDAR ID
                                // ================================
                                if (id <= 0)
                                {
                                    return BadRequest("ID inválido");
                                }

                                // ================================
                                // VALIDAR MODELO
                                // ================================
                                if (!ModelState.IsValid)
                                {
                                    return BadRequest(ModelState);
                                }

                                // ================================
                                // ASIGNAR ID AL DTO
                                // ================================
                                dto.Id = id;

                                // ================================
                                // ACTUALIZAR
                                // ================================
                                var actualizado = await _usuariosRepo.ActualizarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!actualizado)
                                {
                                    _logger.LogWarning(
                                        "Usuarios con ID {id} no encontrada para actualizar.",
                                        id);

                                    return NotFound($"Usuarios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Usuarios actualizada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Usuarios actualizada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al actualizar Usuarios ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // ELIMINAR
                        // =========================================
                       // [Authorize]
                        [HttpDelete("eliminar/{id}")]
                        [ProducesResponseType(StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> EliminarUsuarios(int id)
                        {
                            try
                            {
                                // ================================
                                // VALIDAR ID
                                // ================================
                                if (id <= 0)
                                {
                                    return BadRequest("ID inválido");
                                }

                                // ================================
                                // ELIMINAR
                                // ================================
                                var eliminado = await _usuariosRepo.EliminarPorIdAsync(id);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!eliminado)
                                {
                                    _logger.LogWarning(
                                        "Usuarios con ID {id} no encontrada para eliminar.",
                                        id);

                                    return NotFound($"Usuarios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Usuarios eliminada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Usuarios eliminada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al eliminar Usuarios ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER TODOS
                        // =========================================
                       // [Authorize]     
                        [HttpGet("Listar")]
                        [ProducesResponseType(typeof(List<UsuariosDto>), StatusCodes.Status200OK)]
                        public async Task<IActionResult> ObtenerTodos(int pagI, int pagF)
                        {
                            try
                            {
                                var lista =
                                    await _usuariosRepo.ObtenerPaginadoAsync(pagI, pagF);

                                return Ok(lista);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Usuarioss");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER POR ID
                        // =========================================
                        // [Authorize]
                        [HttpGet("{id}")]
                        [ProducesResponseType(typeof(UsuariosDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        public async Task<IActionResult> ObtenerPorId(int id)
                        {
                            try
                            {
                                var entidad =
                                    await _usuariosRepo.ObtenerPorIdAsync(id);

                                if (entidad == null)
                                {
                                    return NotFound(
                                        $"No existe usuarios con ID {id}");
                                }

                                return Ok(entidad);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Usuarios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                    // =========================================
                    // EXISTE POR CRITERIO
                    // =========================================
                  //  [Authorize]
                    [HttpGet("existe/{criterio}")]
                    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
                    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                    public async Task<IActionResult> BuscarExisteCriterio(string criterio)
                    {
                        try
                        {
                            bool existe =
                                await _usuariosRepo.ExisteAsync(criterio);

                            return Ok(existe);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al verificar existencia de Usuarios");

                            return StatusCode(
                                500,
                                "Error interno del servidor");
                        }
                    }
                }
            }