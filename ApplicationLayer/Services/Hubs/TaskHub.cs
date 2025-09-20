using Microsoft.AspNetCore.SignalR;

namespace TaskManager.Hubs
{
    public class TaskHub : Hub
    {
        // M�todo para enviar notificaci�n a todos los clientes
        public async Task NotifyNewTask(string description)
        {
            await Clients.All.SendAsync("ReceiveNewTask", description);
        }
    }
}