using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]

    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libros =  await context.Libros.Include(l=>l.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<LibroDTO>(libros);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacioDTO libroCreacioDTO)
        {
            var autoresId = await context.Autores
                .Where(a => libroCreacioDTO.AutoresId.Contains(a.Id))
                .Select(x => x.Id).ToListAsync();

            if (libroCreacioDTO.AutoresId.Count != autoresId.Count)
            {
                return BadRequest("no existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacioDTO);
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
