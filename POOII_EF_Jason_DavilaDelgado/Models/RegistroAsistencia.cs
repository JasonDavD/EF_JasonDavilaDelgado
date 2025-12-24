using System.ComponentModel.DataAnnotations;

namespace POOII_EF_Jason_DavilaDelgado.Models
{
    public class RegistroAsistencia
    {
        [Required]
        public int NumeroRegistro { get; set; }
        public string CodigoSeminario { get; set; }
        public string CodigoEstudiante { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
