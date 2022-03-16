namespace Domain.Entities
{
    public class ConteudoArquivo
    {
        public Remetente Remetente { get; set; }

        public Destinatario Destinatario { get; set; }
        
        public Ordem Ordem { get; set; }
    }
}
