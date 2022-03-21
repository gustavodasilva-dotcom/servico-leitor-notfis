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
                        var tags = await _clienteService.ObterTagsClienteAsync(caminhoCliente.ClienteID);
                        
                        if (tags != null)
                            _arquivoService.VerificarArquivos(caminhoCliente, tags.ToList());
                    }
                }
            }
            catch (Exception) { throw; }
        }
    }
}
