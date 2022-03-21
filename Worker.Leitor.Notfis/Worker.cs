using System;
using System.Threading;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Worker.Leitor.Notfis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IBaseService _baseService;

        private readonly IConfiguration _configuration; 

        public Worker(ILogger<Worker> logger, IBaseService baseService, IConfiguration configuration)
        {
            _logger = logger;

            _baseService = baseService;

            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{DateTimeOffset.Now} - Iniciando execução.");

                try
                {
                    await _baseService.ExecutarServicoAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError($"{DateTimeOffset.Now} - O seguinte erro ocorreu: {e.Message}");
                }
                finally
                {
                    _logger.LogInformation($"{DateTimeOffset.Now} - Processo executado.");
                    _logger.LogInformation($"{DateTimeOffset.Now} - Aguardando próxima execucação.");

                    await Task.Delay(int.Parse(_configuration["Leitor:Configuracoes:Delay"]), stoppingToken);
                }
            }
        }
    }
}
