using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentarioController(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = applicationDbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId) {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var comentario = await context.Comentarios
                .Where(c => c.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentario);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO) {
            var emailClaim = HttpContext.User.Claims.Where(c => c.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var user = await userManager.FindByEmailAsync(email);
            var usuariId = user.Id;

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro) {
                return NotFound();
            }
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuariId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            var cometarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new { id = comentario.Id, libroId = comentario.LibroId }, cometarioDTO);
        }

        [HttpGet("{id:int}",Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id) {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(c => c.Id == id);
            if (comentario == null) { return NotFound(); }
            return mapper.Map<ComentarioDTO>(comentario);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO) {
            var existeLibro = await context.Libros.AnyAsync(l => l.Id == libroId);
            if (!existeLibro) { return NotFound();  }

            var existeComentario = await context.Comentarios.AnyAsync(c => c.Id == id);
            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.Id = id;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
