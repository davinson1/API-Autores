using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]

    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libros = await context.Libros
                .Include(l => l.Comentarios)
                .Include(l => l.autoresLibros)
                .ThenInclude(a => a.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
            libros.autoresLibros = libros.autoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libros);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacioDTO libroCreacioDTO)
        {
            if (libroCreacioDTO.AutoresIds == null) {
                return BadRequest("no se puede crear libro sin autores");
            }

            var autoresIds = await context.Autores
                .Where(a => libroCreacioDTO.AutoresIds.Contains(a.Id))
                .Select(x => x.Id).ToListAsync();

            if (libroCreacioDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("no existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacioDTO);
            AsignarOrdenAutores(libro);
            context.Add(libro);
            await context.SaveChangesAsync();
            var libroDTO = mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacioDTO libroCreacioDTO) {
            var libroDB = await context.Libros
                .Include(x => x.autoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacioDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro) {
            if (libro.autoresLibros != null)
            {
                for (int i = 0; i < libro.autoresLibros.Count; i++)
                {
                    libro.autoresLibros[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument) {

            if (patchDocument == null)
            {
                return BadRequest();
            }
            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO,libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
