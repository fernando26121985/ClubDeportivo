using ClubDeportivo.DTOs;
               using ClubDeportivo.Repositories.Interfaces;
        
                using Microsoft.AspNetCore.Authorization;
                using Microsoft.AspNetCore.Mvc;

                namespace ClubDeportivo.Controller
                {
                    [Route("api/aportes_socios")]
                    [ApiController]
                    public class Aportes_sociosController : ControllerBase
                    {
                        private readonly IAportes_sociosRepository _aportes_sociosRepo;
                       
                        private readonly ILogger<Aportes_sociosController> _logger;

                        public Aportes_sociosController(
                            IAportes_sociosRepository aportes_sociosRepo,
                         
                            ILogger<Aportes_sociosController> logger)
                        {
                            _aportes_sociosRepo = aportes_sociosRepo;
                         
                            _logger = logger;
                        }

                        // =========================================
                        // INSERTAR
                        // =========================================
                        [AllowAnonymous]
                        [HttpPost("insertar")]
                        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> InsertarAportes_socios(
                            [FromBody] Aportes_sociosDto dto)
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
                                    await _aportes_sociosRepo.InsertarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (idInsertado <= 0)
                                {
                                    _logger.LogWarning(
                                        "No se pudo insertar la Aportes_socios.");

                                    return StatusCode(
                                        500,
                                        "No se pudo insertar la Aportes_socios.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Aportes_socios insertada correctamente. ID: {id}",
                                    idInsertado);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Aportes_socios insertada correctamente",
                                    id = idInsertado
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al insertar Aportes_socios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // MODIFICAR
                        // =========================================
                        [AllowAnonymous]
                        [HttpPut("actualizar/{id}")]
                        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> ActualizarAportes_socios(int id, [FromBody] Aportes_sociosDto dto)
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
                                var actualizado = await _aportes_sociosRepo.ActualizarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!actualizado)
                                {
                                    _logger.LogWarning(
                                        "Aportes_socios con ID {id} no encontrada para actualizar.",
                                        id);

                                    return NotFound($"Aportes_socios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Aportes_socios actualizada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Aportes_socios actualizada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al actualizar Aportes_socios ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // ELIMINAR
                        // =========================================
                        [AllowAnonymous]
                        [HttpDelete("eliminar/{id}")]
                        [ProducesResponseType(StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status400BadRequest)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                        public async Task<IActionResult> EliminarAportes_socios(int id)
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
                                var eliminado = await _aportes_sociosRepo.EliminarPorIdAsync(id);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!eliminado)
                                {
                                    _logger.LogWarning(
                                        "Aportes_socios con ID {id} no encontrada para eliminar.",
                                        id);

                                    return NotFound($"Aportes_socios con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Aportes_socios eliminada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Aportes_socios eliminada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al eliminar Aportes_socios ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER TODOS
                        // =========================================
                        [HttpGet("Listar")]
                        [ProducesResponseType(typeof(List<Aportes_sociosDto>), StatusCodes.Status200OK)]
                        public async Task<IActionResult> ObtenerTodos(int pagI, int pagF)
                        {
                            try
                            {
                                var lista =
                                    await _aportes_sociosRepo.ObtenerPaginadoAsync(pagI, pagF);

                                return Ok(lista);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Aportes_socioss");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER POR ID
                        // =========================================
                        [HttpGet("{id}")]
                        [ProducesResponseType(typeof(Aportes_sociosDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        public async Task<IActionResult> ObtenerPorId(int id)
                        {
                            try
                            {
                                var entidad =
                                    await _aportes_sociosRepo.ObtenerPorIdAsync(id);

                                if (entidad == null)
                                {
                                    return NotFound(
                                        $"No existe aportes_socios con ID {id}");
                                }

                                return Ok(entidad);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Aportes_socios");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                    // =========================================
                    // EXISTE POR CRITERIO
                    // =========================================
                    [HttpGet("existe/{criterio}")]
                    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
                    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
                    public async Task<IActionResult> BuscarExisteCriterio(string criterio)
                    {
                        try
                        {
                            bool existe =
                                await _aportes_sociosRepo.ExisteAsync(criterio);

                            return Ok(existe);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al verificar existencia de Aportes_socios");

                            return StatusCode(
                                500,
                                "Error interno del servidor");
                        }
                    }
                }
            }