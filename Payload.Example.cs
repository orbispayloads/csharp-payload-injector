using System;
using System.Net;
using System.Threading.Tasks;
using libdebug;

namespace OrbisPayloads
{
    public partial class mainFrm : Form
    {
        private static FRAME4 console;

        private void ConnectConsole()
        {
            console = new FRAME4("192.168.137.1");
            console.Connect();
        }

        private async void executePayload(string payload)
        {
            await SendPayloadAsync(Helpers.Settings.Get("target_address"), "payload.bin");
        }
    }
}
