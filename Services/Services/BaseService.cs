using System;
using System.Linq;
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

                if (caminhosClientes.Any())
                {
                    foreach (var caminhoCliente in caminhosClientes)
                    {
                        var extensao = await _clienteService.ObterClientesExtensoesAsync(caminhoCliente.Cliente_LayoutID);

                        if (extensao != null)
                        {
                            var tags = await _clienteService.ObterTagsClienteAsync(caminhoCliente.Cliente_LayoutID);

                            if (tags != null)
                                _arquivoService.VerificarArquivos(caminhoCliente, extensao, tags.ToList());
                        }
                    }
                }
            }
            catch (Exception) { throw; }
        }
    }
}
