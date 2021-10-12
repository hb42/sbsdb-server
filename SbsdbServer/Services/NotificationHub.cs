using System.Threading.Tasks;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Services {
    public class NotificationHub: Hub {
        public const string ApChangeEvent = "apchange";
        private readonly ILogger<NotificationHub> _log;

        public NotificationHub(ILogger<NotificationHub> log) {
            _log = log;
            _log.LogDebug("c'tor NotificationHub");
        }

        // public Task Testmessage(string data) {
        //     _log.LogDebug("### received 'testmessage' " + data);
        //     return Clients.All.SendAsync("testanswer", data);
        // }
    }
}
