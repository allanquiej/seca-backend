namespace SecaBackend.Models
{
    public class ConsultorInput
    {
        public string Pregunta { get; set; } = string.Empty;
    }

    public class ConsultorRespuesta
    {
        public string PreguntaOriginal { get; set; } = string.Empty;
        public string Respuesta { get; set; } = string.Empty;
    }
}
