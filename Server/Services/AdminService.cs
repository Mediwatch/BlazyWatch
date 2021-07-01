using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Server.Services {
    public class AdminService : BackgroundService {
        public bool isInDemo { get; set; } = false;
        public bool canPay { get; set; } = false;
        public string[] articleNew { get; set; } = null;

        private IServiceScopeFactory scopeFactory;

        public AdminService (IServiceScopeFactory scopeFactory) {
            this.scopeFactory = scopeFactory;
        }
        protected override Task ExecuteAsync (CancellationToken stoppingToken) {
            throw new System.NotImplementedException ();
        }
    }
}