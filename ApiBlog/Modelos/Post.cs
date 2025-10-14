using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Modelos
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "La descripción no puede estar vacía.")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "Debes añadir al menos una etiqueta.")]
        public string Etiquetas { get; set; } = "";
        public string? RutaImagen { get; set; }
        [Required(ErrorMessage= "Hubo un problema con la fecha")]
        public DateTimeOffset FechaCreacion{ get; set; } = DateTimeOffset.Now;
        public DateTimeOffset FechaActualizacion { get; set; }

    }
}
