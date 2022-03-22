using System;

namespace Domain.Entities
{
    public class InformacoesLinhas
    {
        public string Descricao { get; set; }

        public int ID { get; set; }

        public int Linha { get; set; }

        public int LayoutID { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime Data { get; set; }
    }
}
