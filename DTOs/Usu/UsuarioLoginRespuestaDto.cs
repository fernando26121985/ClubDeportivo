using ClubDeportivo.Models;
namespace ClubDeportivo.DTOs.Usu
{
    public class UsuarioLoginRespuestaDto
    {
        public UsuariosDto Usuario { get; set; }
        public string Token { get; set; }
    }
}
