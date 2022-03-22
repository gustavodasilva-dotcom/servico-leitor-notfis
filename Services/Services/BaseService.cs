using System;
using System.Linq;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public class BaseService : IBaseService
    {
        private readonly ILogger<BaseService> _logger;

        private readonly IArquivoService _arquivoService;

        private readonly IClienteService _clienteService;

        public BaseService(ILogger<BaseService> logger, IArquivoService arquivoService, IClienteService clienteService)
        {
            _logger = logger;

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
                        {
                            var informacoesLinhas = await _clienteService.ObterInformacoesLinhasAsync(caminhoCliente.ClienteID);

                            if (informacoesLinhas != null)
                                _arquivoService.VerificarArquivos(caminhoCliente, tags.ToList(), informacoesLinhas.ToList());
                            else
                                _logger.LogInformation($"{DateTimeOffset.Now} - Não foram encontradas as informações de linha do cliente {caminhoCliente.ClienteID}.");
                        }
                        else
                            _logger.LogInformation($"{DateTimeOffset.Now} - O cliente {caminhoCliente.ClienteID} não possui tags cadastradas.");
                    }
                }
                else
                    _logger.LogInformation($"{DateTimeOffset.Now} - Não há caminhos de clientes cadastrados.");
            }
            catch (Exception) { throw; }
        }
    }
}
