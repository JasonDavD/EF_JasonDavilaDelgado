using System.ComponentModel.DataAnnotations;

namespace POOII_EF_Jason_DavilaDelgado.Models
{
    public class Seminario
    {
        [Required]
        public string CodigoSeminario { get; set; }

        public string NombreCurso { get; set; }

        public string HorarioClase { get; set; }

        public int CapacidadSeminario { get; set; }

        public string FotoUrl { get; set; }
    }
}
