using System.ComponentModel.DataAnnotations;

namespace ClienteBlogWASM.Modelos.ViewModels
{
    public class PostCrearVM
    {
        [Required]
        [StringLength(80, ErrorMessage = "El título no puede exceder los 80 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        public string RutaImagen { get; set; }

        [Required(ErrorMessage = "Las etiquetas son obligatorias")]
        public string Etiquetas { get; set; } = string.Empty;
    }
}
