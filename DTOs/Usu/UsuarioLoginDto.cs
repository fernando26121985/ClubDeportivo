using System.ComponentModel.DataAnnotations;
namespace ClubDeportivo.DTOs.Usu
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string Email { get; set; }


        [Required(ErrorMessage = "la Contraseña es obligatorio")]
        public string Clave { get; set; }
    }
}
