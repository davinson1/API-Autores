using WebAPIAutores.DTOs;

namespace WebAPIAutores.Utilidades
{
    public static class IqueribleExtensions
    {

        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordPorPagina)
                .Take(paginacionDTO.RecordPorPagina);
        }
    }
}
