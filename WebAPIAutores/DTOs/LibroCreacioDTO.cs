using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacioDTO
    {

        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength: 120)]
        public string Titulo { get; set; }
        public List<int> AutoresId { get; set; }
    }
}
