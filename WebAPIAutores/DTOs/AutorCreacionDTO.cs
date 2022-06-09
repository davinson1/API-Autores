using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [PrimeraLetraMayusculaAttribute]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
        public string Name { get; set; }
    }
}
