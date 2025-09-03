using Microsoft.AspNetCore.SignalR;

namespace ShopApi.Hubs
{
    public class OrderHub : Hub
    {
        public async Task JoinAdminGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        }
    }
}