using System.ComponentModel.DataAnnotations;

namespace ClubDeportivo.DTOs
{
    /// <summary>
    /// DTO generado para usuarios
    /// Data Transfer Object
    /// </summary>
        public class UsuariosDto
        {
        public int Id { get; set; }
        public int? SocioId { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
    }
}