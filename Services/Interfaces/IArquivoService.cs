using Domain.Entities;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IArquivoService
    {
        void VerificarArquivos(PastasClientes pastaCliente, Extensoes extensao, List<Clientes_Layouts_Tag> tags);
    }
}
