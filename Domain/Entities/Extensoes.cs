using System;

namespace Domain.Entities
{
    public class Extensoes
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Extensao { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime Data { get; set; }
    }
}
