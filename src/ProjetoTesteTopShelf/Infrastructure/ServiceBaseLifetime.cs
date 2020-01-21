using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace ProjetoTesteTopShelf.Infrastructure
{
    public class ServiceBaseLifetime : ServiceControl
    {
        private Timer _timer;

        public bool Start(HostControl hostControl)
        {
            _timer = new Timer(async c => await ExecuteAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return true;
        }

        private async Task ExecuteAsync()
        {
            try
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                //Início da execução
            }
            finally
            {
                _timer.Change(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
            }
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
