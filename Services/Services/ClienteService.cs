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

        public async Task<IEnumerable<Clientes_Tags>> ObterTagsClienteAsync(int clienteID)
        {
            return await _clienteRepository.ObterTagsClienteAsync(clienteID);
        }

        public async Task<IEnumerable<InformacoesLinhas>> ObterInformacoesLinhasAsync(int clienteID)
        {
            return await _clienteRepository.ObterInformacoesLinhasAsync(clienteID);
        }
    }
}
