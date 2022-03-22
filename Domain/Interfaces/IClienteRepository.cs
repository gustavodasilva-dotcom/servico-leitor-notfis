using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<PastasClientes>> ObterCaminhosAsync();

        Task<IEnumerable<Clientes_Tags>> ObterTagsClienteAsync(int clienteID);

        Task<IEnumerable<InformacoesLinhas>> ObterInformacoesLinhasAsync(int clienteID);
    }
}
