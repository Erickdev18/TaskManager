using Microsoft.AspNetCore.SignalR;

namespace TaskManager.Hubs
{
    public class TaskHub : Hub
    {
        // Método para enviar notificación a todos los clientes
        public async Task NotifyNewTask(string description)
        {
            await Clients.All.SendAsync("ReceiveNewTask", description);
        }
    }
}