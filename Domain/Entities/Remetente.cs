namespace Domain.Entities
{
    public class Remetente
    {
        public string Identificacao { get; set; }

        public string InscricaoEstadual { get; set; }

        public string RazaoSocial { get; set; }

        public Endereco Endereco { get; set; }
    }
}
