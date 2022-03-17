using System;

namespace Domain.Entities
{
    public class PastasClientes
    {
        public int ID { get; set; }

        public int Cliente_LayoutID { get; set; }

        public string CaminhoNovosArquivos { get; set; }

        public string CaminhoProcessados { get; set; }

        public string CaminhoErro { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime Data { get; set; }
    }
}
