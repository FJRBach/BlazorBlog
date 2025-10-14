using System.ComponentModel.DataAnnotations;

namespace ClienteBlogWASM.Modelos
{
    public class UsuarioAutenticacion
    {
        [Required(ErrorMessage = "Verificar datos ingresados")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El password o usuario es incorrecto")]
        public string Password { get; set; }
    }
}
