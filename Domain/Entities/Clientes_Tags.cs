using System;

namespace Domain.Entities
{
    public class Clientes_Tags
    {
        public int ID { get; set; }

        public int TagID { get; set; }

        public bool Obrigatoria { get; set; }

        public int IndiceInicial { get; set; }

        public int Tamanho { get; set; }

        public int ClienteID { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime Data { get; set; }
    }
}
