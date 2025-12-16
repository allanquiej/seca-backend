// Esta clase representa un posible cliente que deja sus datos en el sitio.

namespace SecaBackend.Models
{
    public class Lead
    {
        public int Id { get; set; }

        // Nombre de la persona interesada.
        public string Nombre { get; set; } = string.Empty;

        // Teléfono de contacto (puede ser null).
        public string? Telefono { get; set; }

        // Correo electrónico (puede ser null).
        public string? Email { get; set; }

        // Mensaje libre que escribió la persona.
        public string? Mensaje { get; set; }

        // Fecha en que se registró el lead.
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
