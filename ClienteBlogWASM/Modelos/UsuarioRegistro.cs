using System.ComponentModel.DataAnnotations;

namespace ClienteBlogWASM.Modelos
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage="Verificar datos ingresados")]
        public string NombreUsuario {  get; set; }
        [Required(ErrorMessage="Verificar datos ingresados")]
        public string Nombre { get; set; }
        [Required(ErrorMessage="Verificar datos ingresados")]
        public string Email { get; set; }
        [Required(ErrorMessage="Verificar datos ingresados")]
        public string Password {  get; set; }
    }
}
