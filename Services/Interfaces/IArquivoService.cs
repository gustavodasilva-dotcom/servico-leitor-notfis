using Domain.Entities;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IArquivoService
    {
        void VerificarArquivos(PastasClientes pastaCliente, List<Clientes_Tags> tags);
    }
}
