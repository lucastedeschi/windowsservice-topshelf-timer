using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoTesteTopShelf.Infrastructure;
using Topshelf;

namespace ProjetoTesteTopShelf
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder().
                UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddSingleton(typeof(ServiceBaseLifetime));
                });

            HostFactory.Run(x =>
            {
                x.Service<ServiceBaseLifetime>(sc =>
                {
                    sc.ConstructUsing(s => host.Build().Services.GetRequiredService<ServiceBaseLifetime>());
                    sc.WhenStarted((s, c) => s.Start(c));
                    sc.WhenStopped((s, c) => s.Stop(c));
                });

                x.RunAsLocalSystem()
                    .DependsOnEventLog()
                    .StartAutomatically()
                    .EnableServiceRecovery(rc => rc.RestartService(1));

                x.SetDescription("Teste do TopShelf");
                x.SetDisplayName("Projeto Teste TopShelf");
                x.SetServiceName("ProjetoTeste.TopShelf");
            });

            await Task.CompletedTask;
        }
    }
}
