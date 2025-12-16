// Esta clase representa la tabla "ChatLogs" en la base de datos.

namespace SecaBackend.Models
{
    public class ChatLog
    {
        // Clave primaria de la tabla.
        public int Id { get; set; }

        // Nombre o identificador del usuario (puede ser null).
        public string? Usuario { get; set; }

        // Pregunta que el usuario hizo al chatbot.
        public string Pregunta { get; set; } = string.Empty;

        // Respuesta que dio el sistema.
        public string Respuesta { get; set; } = string.Empty;

        // Momento en que ocurrió esta interacción.
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
