using ClubDeportivo.DTOs;
               using ClubDeportivo.Repositories.Interfaces;
        
                using Microsoft.AspNetCore.Authorization;
                using Microsoft.AspNetCore.Mvc;

                namespace ClubDeportivo.Controller
                {
                    [Route("api/deudas_anteriores")]
                    [ApiController]
                    public class Deudas_anterioresController : ControllerBase
                    {
                        private readonly IDeudas_anterioresRepository _deudas_anterioresRepo;
                      
                        private readonly ILogger<Deudas_anterioresController> _logger;

                        public Deudas_anterioresController(
                            IDeudas_anterioresRepository deudas_anterioresRepo,
                       
                            ILogger<Deudas_anterioresController> logger)
                        {
                            _deudas_anterioresRepo = deudas_anterioresRepo;
                          
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
                        public async Task<IActionResult> InsertarDeudas_anteriores(
                            [FromBody] Deudas_anterioresDto dto)
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
                                    await _deudas_anterioresRepo.InsertarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (idInsertado <= 0)
                                {
                                    _logger.LogWarning(
                                        "No se pudo insertar la Deudas_anteriores.");

                                    return StatusCode(
                                        500,
                                        "No se pudo insertar la Deudas_anteriores.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Deudas_anteriores insertada correctamente. ID: {id}",
                                    idInsertado);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Deudas_anteriores insertada correctamente",
                                    id = idInsertado
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al insertar Deudas_anteriores");

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
                        public async Task<IActionResult> ActualizarDeudas_anteriores(int id, [FromBody] Deudas_anterioresDto dto)
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
                                var actualizado = await _deudas_anterioresRepo.ActualizarAsync(dto);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!actualizado)
                                {
                                    _logger.LogWarning(
                                        "Deudas_anteriores con ID {id} no encontrada para actualizar.",
                                        id);

                                    return NotFound($"Deudas_anteriores con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Deudas_anteriores actualizada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Deudas_anteriores actualizada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al actualizar Deudas_anteriores ID: {id}", id);

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
                        public async Task<IActionResult> EliminarDeudas_anteriores(int id)
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
                                var eliminado = await _deudas_anterioresRepo.EliminarPorIdAsync(id);

                                // ================================
                                // VALIDAR RESULTADO
                                // ================================
                                if (!eliminado)
                                {
                                    _logger.LogWarning(
                                        "Deudas_anteriores con ID {id} no encontrada para eliminar.",
                                        id);

                                    return NotFound($"Deudas_anteriores con ID {id} no encontrada.");
                                }

                                // ================================
                                // LOG
                                // ================================
                                _logger.LogInformation(
                                    "Deudas_anteriores eliminada correctamente. ID: {id}",
                                    id);

                                // ================================
                                // RESPUESTA
                                // ================================
                                return Ok(new
                                {
                                    mensaje = "Deudas_anteriores eliminada correctamente",
                                    id = id
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al eliminar Deudas_anteriores ID: {id}", id);

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER TODOS
                        // =========================================
                        [HttpGet("Listar")]
                        [ProducesResponseType(typeof(List<Deudas_anterioresDto>), StatusCodes.Status200OK)]
                        public async Task<IActionResult> ObtenerTodos(int pagI, int pagF)
                        {
                            try
                            {
                                var lista =
                                    await _deudas_anterioresRepo.ObtenerPaginadoAsync(pagI, pagF);

                                return Ok(lista);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Deudas_anterioress");

                                return StatusCode(
                                    500,
                                    "Error interno del servidor");
                            }
                        }

                        // =========================================
                        // OBTENER POR ID
                        // =========================================
                        [HttpGet("{id}")]
                        [ProducesResponseType(typeof(Deudas_anterioresDto), StatusCodes.Status200OK)]
                        [ProducesResponseType(StatusCodes.Status404NotFound)]
                        public async Task<IActionResult> ObtenerPorId(int id)
                        {
                            try
                            {
                                var entidad =
                                    await _deudas_anterioresRepo.ObtenerPorIdAsync(id);

                                if (entidad == null)
                                {
                                    return NotFound(
                                        $"No existe deudas_anteriores con ID {id}");
                                }

                                return Ok(entidad);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error al obtener Deudas_anteriores");

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
                                await _deudas_anterioresRepo.ExisteAsync(criterio);

                            return Ok(existe);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error al verificar existencia de Deudas_anteriores");

                            return StatusCode(
                                500,
                                "Error interno del servidor");
                        }
                    }
                }
            }