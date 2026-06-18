using ClubDeportivo.DTOs;
               using ClubDeportivo.Repositories.Interfaces;
             
                using Microsoft.AspNetCore.Authorization;
                using Microsoft.AspNetCore.Mvc;

                namespace ClubDeportivo.Controller
                {
                    [Route("api/periodos")]
                    [ApiController]
                    public class PeriodosController : ControllerBase
                    {
                        private readonly IPeriodosRepository _periodosRepo;
                       
                        private readonly ILogger<PeriodosController> _logger;

                        public PeriodosController(
                            IPeriodosRepository periodosRepo,
                        
                            ILogger<PeriodosController> logger)
                        {
                            _periodosRepo = periodosRepo;
                       
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
                        public async Task<IActionResult> InsertarPeriodos(
                            [FromBody] PeriodosDto dto)
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
                                    await _periodosRepo.InsertarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (idInsertado <= 0)
                                {
                                    _logger.LogWarning(
                                        "No se pudo insertar la Periodos.");

                                    return StatusCode(
                                        500,
                                        "No se pudo insertar la Periodos.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Periodos insertada correctamente. ID: {id}",
                                    idInsertado);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Periodos insertada correctamente",
                                    id = idInsertado
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al insertar Periodos");

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
                        public async Task<IActionResult> ActualizarPeriodos(int id, [FromBody] PeriodosDto dto)
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
                                var actualizado = await _periodosRepo.ActualizarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!actualizado)
                                {
                                    _logger.LogWarning(
                                        "Periodos con ID {id} no encontrada para actualizar.",
                                        id);

                                    return NotFound($"Periodos con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Periodos actualizada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Periodos actualizada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al actualizar Periodos ID: {id}", id);

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
                        public async Task<IActionResult> EliminarPeriodos(int id)
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
                                var eliminado = await _periodosRepo.EliminarPorIdAsync(id);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!eliminado)
                                {
                                    _logger.LogWarning(
                                        "Periodos con ID {id} no encontrada para eliminar.",
                                        id);

                                    return NotFound($"Periodos con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Periodos eliminada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Periodos eliminada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al eliminar Periodos ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER TODOS
                        // =========================================
                        [HttpGet("Listar")]
                        [ProducesResponseType(typeof(List<PeriodosDto>), StatusCodes.Status200OK)]
                        public async Task<IActionResult> ObtenerTodos(int pagI, int pagF)
                        {
                            try
                            {
                                var lista =
                                    await _periodosRepo.ObtenerPaginadoAsync(pagI, pagF);

                                return Ok(lista);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Periodoss");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER POR ID
                        // =========================================
                        [HttpGet("{id}")]
                        [ProducesResponseType(typeof(PeriodosDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        public async Task<IActionResult> ObtenerPorId(int id)
                        {
                            try
                            {
                                var entidad =
                                    await _periodosRepo.ObtenerPorIdAsync(id);

                                if (entidad == null)
                                {
                                    return NotFound(
                                        $"No existe periodos con ID {id}");
                                }

                                return Ok(entidad);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Periodos");

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
                                await _periodosRepo.ExisteAsync(criterio);

                            return Ok(existe);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al verificar existencia de Periodos");

                            return StatusCode(
                                500,
                                "Error interno del servidor");
                        }
                    }
                }
            }