using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisPayloads
{
  internal class Injector
  {
            public static async Task<bool> SendPayloadAsync(string ipAddress, string payloadFileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string payloadFilePath = Path.Combine(baseDirectory, "Payloads", payloadFileName);
            byte[] payloadData;

            try
            {
                payloadData = File.ReadAllBytes(payloadFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to read payload file: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!await IsServerReady(ipAddress))
            {
                MessageBox.Show("Server is not ready for payloads.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return await UploadPayloadAsync(ipAddress, payloadData);
        }

        private static async Task<bool> IsServerReady(string ipAddress)
        {
            using (var webClient = new WebClient())
            {
                string statusUrl = $"http://{ipAddress}:9090/status";
                try
                {
                    string statusData = await webClient.DownloadStringTaskAsync(statusUrl);
                    return statusData.Contains("{ \"status\": \"ready\" }");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking server status: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private static async Task<bool> UploadPayloadAsync(string ipAddress, byte[] payloadData)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    string uploadUrl = $"http://{ipAddress}:9090";
                    await webClient.UploadDataTaskAsync(uploadUrl, payloadData);

                    MessageBox.Show($"Payload was loaded successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending the payload: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
  }
}
