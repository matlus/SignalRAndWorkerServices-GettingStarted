using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRGettingStarted.WorkerServices
{
    public sealed class MessageBrokerPubSubWorker : BackgroundService
    {
        private readonly IHubContext<MessageBrokerHub> _messageBrokerHubContext;

        public MessageBrokerPubSubWorker(IHubContext<MessageBrokerHub> messageBrokerHubContext)
        {
            _messageBrokerHubContext = messageBrokerHubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                var eventMessage = new EventMessage($"Id_{ Guid.NewGuid():N}", $"Title_{Guid.NewGuid():N}", DateTime.UtcNow);
                await _messageBrokerHubContext.Clients.All.SendAsync("onMessageReceived", eventMessage, stoppingToken);
            }
        }
    }
}
