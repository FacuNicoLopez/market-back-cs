using Microsoft.Identity.Client;

namespace ApiUsuarios.Models{
    public class UsuarioDto{
        public string Email { get; set; }
        public string Password { get; set; }
        public string NameUser { get; set; }
        public string Apellido { get; set; }
    }

    public class NameUserDto
    {
        
        public string NameUser { get; set; }
    }

    public class BackgroundImageDTO
    {
        public string BackgroundImage { get; set; }
    }

    public class UsuarioEmailDto
    {
        public string Email { get; set; }
    }
}