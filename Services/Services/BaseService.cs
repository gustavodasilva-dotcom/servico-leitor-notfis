using System;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services.Services
{
    public class BaseService : IBaseService
    {
        private readonly IArquivoService _arquivoService;

        private readonly IClienteService _clienteService;

        public BaseService(IArquivoService arquivoService, IClienteService clienteService)
        {
            _arquivoService = arquivoService;

            _clienteService = clienteService;
        }

        public async Task ExecutarServicoAsync()
        {
            try
            {
                var caminhosClientes = await _clienteService.ObterCaminhosAsync();

                foreach (var caminhoCliente in caminhosClientes)
                {
                    var extensao = await _clienteService.ObterClientesExtensoesAsync(caminhoCliente.Cliente_LayoutID);

                    _arquivoService.VerificarArquivos(caminhoCliente, extensao);
                }
            }
            catch (Exception) { throw; }
        }
    }
}
