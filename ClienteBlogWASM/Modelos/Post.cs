using System.ComponentModel.DataAnnotations;

namespace ClienteBlogWASM.Modelos
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El title es obligatorio")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        public string? RutaImagen { get; set; }
        [Required(ErrorMessage = "Las etiquetas son obligatorias")]
        public string Etiquetas { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaActualizacion { get; set; }
    }
}
