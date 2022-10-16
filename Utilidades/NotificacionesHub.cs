using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Web.Utilidades
{
    public class NotificacionesHub:Hub
    {
        public async Task Send(string mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", mensaje);
        }
    }
}
