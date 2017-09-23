using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace Selly.NMS.Web.Hubs
{
    public class HomeHub : Hub
    {
        public static HomeHub instance;
        public HomeHub()
        {
            instance = this;
        }

        public static List<string> ConnectedUsers;

        public void Send(string originatorUser, string message)
        {
            Clients.All.messageReceived(originatorUser, message);
        }

        public void Connect(string newUser)
        {
            if (ConnectedUsers == null)
                ConnectedUsers = new List<string>();

            ConnectedUsers.Add(newUser);
            Clients.Caller.getConnectedUsers(ConnectedUsers);
            Clients.Others.newUserAdded(newUser);
        }
    }
}
