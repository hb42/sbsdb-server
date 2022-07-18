using System.Threading.Tasks;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Services {
    public class NotificationHub: Hub {
        public const string ApChangeEvent = "apchange";
        public const string HwChangeEvent = "hwchange";
        public const string AddHwEvent = "addhw";
        public const string KonfigChangeEvent = "konfigchange";
        public const string ExtProgChangeEvent = "extprogchange";
        public const string AptypChangeEvent = "aptypchange";
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
