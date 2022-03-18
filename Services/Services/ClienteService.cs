using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<PastasClientes>> ObterCaminhosAsync()
        {
            return await _clienteRepository.ObterCaminhosAsync();
        }

        public async Task<Extensoes> ObterClientesExtensoesAsync(int cliente_layoutID)
        {
            return await _clienteRepository.ObterClientesExtensoesAsync(cliente_layoutID);
        }

        public async Task<IEnumerable<Clientes_Layouts_Tag>> ObterTagsClienteAsync(int cliente_layoutID)
        {
            return await _clienteRepository.ObterTagsClienteAsync(cliente_layoutID);
        }
    }
}
