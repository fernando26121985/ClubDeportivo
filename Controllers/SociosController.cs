using ClubDeportivo.DTOs;
               using ClubDeportivo.Repositories.Interfaces;
             
                using Microsoft.AspNetCore.Authorization;
                using Microsoft.AspNetCore.Mvc;

                namespace ClubDeportivo.Controller
                {
                    [Route("api/socios")]
                    [ApiController]
                    public class SociosController : ControllerBase
                    {
                        private readonly ISociosRepository _sociosRepo;
                   
                        private readonly ILogger<SociosController> _logger;

                        public SociosController(
                            ISociosRepository sociosRepo,
                           
                            ILogger<SociosController> logger)
                        {
                            _sociosRepo = sociosRepo;
                        
                            _logger = logger;
                        }

                        // =========================================
                        // INSERTAR
                        // =========================================
                      //  [Authorize]
                        [HttpPost("insertar")]
                        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> InsertarSocios(
                            [FromBody] SociosDto dto)
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
                                    await _sociosRepo.InsertarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (idInsertado <= 0)
                                {
                                    _logger.LogWarning(
                                        "No se pudo insertar la Socios.");

                                    return StatusCode(
                                        500,
                                        "No se pudo insertar la Socios.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Socios insertada correctamente. ID: {id}",
                                    idInsertado);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Socios insertada correctamente",
                                    id = idInsertado
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al insertar Socios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // MODIFICAR
                        // =========================================
                      //  [Authorize]
                        [HttpPut("actualizar/{id}")]
                        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> ActualizarSocios(int id, [FromBody] SociosDto dto)
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
                                var actualizado = await _sociosRepo.ActualizarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!actualizado)
                                {
                                    _logger.LogWarning(
                                        "Socios con ID {id} no encontrada para actualizar.",
                                        id);

                                    return NotFound($"Socios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Socios actualizada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Socios actualizada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al actualizar Socios ID: {id}", id);

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
                        public async Task<IActionResult> EliminarSocios(int id)
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
                                var eliminado = await _sociosRepo.EliminarPorIdAsync(id);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!eliminado)
                                {
                                    _logger.LogWarning(
                                        "Socios con ID {id} no encontrada para eliminar.",
                                        id);

                                    return NotFound($"Socios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Socios eliminada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Socios eliminada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al eliminar Socios ID: {id}", id);

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
                        [ProducesResponseType(typeof(List<SociosDto>), StatusCodes.Status200OK)]
                        public async Task<IActionResult> ObtenerTodos(int pagI, int pagF)
                        {
                            try
                            {
                                var lista =
                                    await _sociosRepo.ObtenerPaginadoAsync(pagI, pagF);

                                return Ok(lista);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Socioss");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }
                    // =========================================
                    // OBTENER TODOS
                    // =========================================
                    // [Authorize]
                    [HttpGet("ListarTodos")]
                    [ProducesResponseType(typeof(List<SociosDto>), StatusCodes.Status200OK)]
                    public async Task<IActionResult> ListarTodos()
                    {
                        try
                        {
                            var lista =
                                await _sociosRepo.ObtenerTodosAsync();

                            return Ok(lista);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al obtener Todos los Socioss");

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
                        [ProducesResponseType(typeof(SociosDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        public async Task<IActionResult> ObtenerPorId(int id)
                        {
                            try
                            {
                                var entidad =
                                    await _sociosRepo.ObtenerPorIdAsync(id);

                                if (entidad == null)
                                {
                                    return NotFound(
                                        $"No existe socios con ID {id}");
                                }

                                return Ok(entidad);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Socios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                    // =========================================
                    // EXISTE POR CRITERIO
                    // =========================================
                   // [Authorize]
                    [HttpGet("existe/{criterio}")]
                    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
                    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                    public async Task<IActionResult> BuscarExisteCriterio(string criterio)
                    {
                        try
                        {
                            bool existe =
                                await _sociosRepo.ExisteAsync(criterio);

                            return Ok(existe);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al verificar existencia de Socios");

                            return StatusCode(
                                500,
                                "Error interno del servidor");
                        }
                    }
                }
            }