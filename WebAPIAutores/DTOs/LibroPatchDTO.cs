using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroPatchDTO
    {
        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength: 120)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
