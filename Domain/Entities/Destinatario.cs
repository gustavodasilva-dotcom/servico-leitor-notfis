namespace Domain.Entities
{
    public class Destinatario
    {
        public string Nome { get; set; }

        public string Identificacao { get; set; }
        
        public string InscricaoEstadual { get; set; }

        public Endereco Endereco { get; set; }
    }
}
