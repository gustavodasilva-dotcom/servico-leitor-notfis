using System;

namespace Domain.Entities
{
    public class Clientes_Layouts_Tag
    {
        public int ID { get; set; }

        public int TagID { get; set; }

        public bool Obrigatoria { get; set; }

        public int InicioIndice { get; set; }

        public int Tamanho { get; set; }

        public string Attribute { get; set; }

        public int Indice { get; set; }

        public int Cliente_LayoutID { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime Data { get; set; }
    }
}
