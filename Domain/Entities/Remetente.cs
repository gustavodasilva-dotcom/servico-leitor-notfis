namespace Domain.Entities
{
    public class Remetente
    {
        public string RazaoSocial { get; set; }

        public string Cnpj { get; set; }

        public Endereco Endereco { get; set; }
    }
}
