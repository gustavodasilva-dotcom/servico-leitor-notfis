using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<PastasClientes>> ObterCaminhosAsync();

        Task<Extensoes> ObterClientesExtensoesAsync(int cliente_layoutID);

        Task<IEnumerable<Clientes_Layouts_Tag>> ObterTagsClienteAsync(int cliente_layoutID);
    }
}
