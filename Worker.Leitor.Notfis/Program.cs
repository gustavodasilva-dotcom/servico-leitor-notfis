using Services.Services;
using Services.Interfaces;
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

                    services.AddHostedService<Worker>();
                });
    }
}
