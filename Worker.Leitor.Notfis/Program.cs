using Domain.Interfaces;
using Services.Services;
using Services.Interfaces;
using Infra.Data.Config;
using Infra.Data.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Leitor.Notfis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IBaseService, BaseService>();
                    services.AddSingleton<IArquivoService, ArquivoService>();
                    services.AddSingleton<IClienteService, ClienteService>();

                    services.AddSingleton<IConnection, Connection>();
                    services.AddSingleton<IClienteRepository, ClienteRepository>();

                    services.AddHostedService<Worker>();
                });
    }
}
