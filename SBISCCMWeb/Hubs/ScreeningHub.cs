using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Hubs
{
    [HubName("screeningHub")]
    public class ScreeningHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ScreeningHub>();
            context.Clients.All.refreshComplianceData();
        }
    }
}