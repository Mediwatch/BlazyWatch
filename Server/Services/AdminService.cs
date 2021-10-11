using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        protected override async Task ExecuteAsync (CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 5);
            }
        }
    }
}