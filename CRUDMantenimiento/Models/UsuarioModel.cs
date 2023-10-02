using System.ComponentModel.DataAnnotations;

namespace CRUDMantenimiento.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo DNI es obligatorio!!")]
        public int? Dni { get; set; }
        [Required(ErrorMessage = "El campo NOMBRE es obligatorio!!")]
        public String? Nombre { get; set; }
        [Required(ErrorMessage = "El campo FECHA es obligatorio!!")]
        public DateTime? Fecha { get; set; }
        [Required(ErrorMessage = "El campo SUELDO es obligatorio!!")]
        public decimal? Sueldo { get; set; }
    }
}

