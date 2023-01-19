using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using IotHubDevice.DTO;


namespace IotHubDevice.repository
{
    public class TelementaryMessage
    {
        private static string connectionString = "HostName=myiotdeviceazure.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=g0j0XdQn554IL/qRY3DXnHd5oTwsYXsMDSM+G577T1M=";
        public static RegistryManager registryManager;
        public static DeviceClient client = null;
        public static string myDeviceConnection = "HostName=myiotdeviceazure.azure-devices.net;DeviceId=iotdevice1;SharedAccessKey=UumAYlM/KmzmQGBq5O7KEvyaf7mHUqBMQ5+8K4Xi3sE=";
        
        public static async Task SendMessage(string deviceName)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                var device = await registryManager.GetTwinAsync(deviceName);
                ReportedProperties properties = new ReportedProperties();
                TwinCollection reportedprop;
                reportedprop = device.Properties.Reported;
                client = DeviceClient.CreateFromConnectionString(myDeviceConnection,Microsoft.Azure.Devices.Client.TransportType.Mqtt);
                while(true)
                {
                    var telementry = new
                    {
                        temprature = reportedprop["temprature"],
                        pressure = reportedprop["pressure"],
                        accuracy = reportedprop["accuracy"],

                    };
                    var telementryString = JsonConvert.SerializeObject(telementry);
                    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(telementryString));
                    await client.SendEventAsync(message);
                    Console.WriteLine("{0}>sending message:{1}", DateTime.Now, telementryString);
                    await Task.Delay(1000);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}