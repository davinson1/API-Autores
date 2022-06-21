namespace WebAPIAutores.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina { get; set; } = 10;
        private readonly int cantidadMaximaPorPagina = 50;

        public int RecordPorPagina
        {
            get 
            {
                return recordsPorPagina; 
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaPorPagina) ? cantidadMaximaPorPagina : value;
            }
        }
    }
}
