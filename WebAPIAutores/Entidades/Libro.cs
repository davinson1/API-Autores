using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayusculaAttribute]
        [StringLength(maximumLength: 120)]
        public string Titulo { get; set; }
        public List<Comentario> Comentarios{ get; set;}
        public List<AutoreLibro> autoresLibros{ get; set;}

    }
}
