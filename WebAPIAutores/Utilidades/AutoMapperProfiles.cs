using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() { 
            CreateMap<AutorCreacionDTO,Autor>();
           
            CreateMap<Autor, AutorDTO>();

            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(a => a.Libros, op => op.MapFrom(MapAutorDTOLibros));
           
            CreateMap<LibroCreacioDTO, Libro>()
                .ForMember(libro => libro.autoresLibros, op => op.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(l => l.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO) {
            var resultado = new List<LibroDTO>();
            if (autor.autoresLibros == null)
            {
                return resultado;
            }

            foreach (var item in autor.autoresLibros)
            {
                resultado.Add(new LibroDTO() { 
                    Id = item.AutorId,
                    Titulo = item.Libro.Titulo
                });
            }
            return resultado;
        }

        private List<AutoreLibro> MapAutoresLibros(LibroCreacioDTO libroCreacioDTO, Libro libro) {
            var resultado = new List<AutoreLibro>();
            if (libroCreacioDTO.AutoresIds == null)
            {
                return resultado;
            }
            foreach (var item in libroCreacioDTO.AutoresIds)
            {
                resultado.Add(new AutoreLibro() { AutorId = item });
            }
            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO) {
            var resultado = new List<AutorDTO>();
            if (libro.autoresLibros == null)
            {
                return resultado;
            }
            foreach (var item in libro.autoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = item.AutorId,
                    Name = item.Autor.Name
                });
            }
            return resultado;
        }
    }
}
