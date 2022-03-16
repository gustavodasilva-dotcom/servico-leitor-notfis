using System.Collections.Generic;

namespace Domain.Entities
{
    public class Ordem
    {
        public string NumeroOrdem { get; set; }

        public List<Item> Items { get; set; }

        public string Preco { get; set; }

        public string ChaveNFe { get; set; }
    }
}
