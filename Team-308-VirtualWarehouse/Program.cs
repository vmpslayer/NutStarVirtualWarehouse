using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client;

namespace Team_308_VirtualWarehouse
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //Console.WriteLine("Connecting...");
            //var mqttFactory = new MqttFactory();

            //using (var mqttClient = mqttFactory.CreateMqttClient())
            //{
            //    var mqttClientOptions = new MqttClientOptionsBuilder()
            //        .WithTcpServer(MqttConfig.Server, MqttConfig.Port)
            //        //.WithCredentials(MqttConfig.User, MqttConfig.Password)
            //        .Build();

            //    mqttClient.ApplicationMessageReceivedAsync += e =>
            //    {
            //        Console.WriteLine("Received application message");

            //        while (e != null)
            //        {
            //            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //            string azimuth = payload.Substring(14, 9);
            //            string elevation = payload.Substring(67, 9);
            //            string distance = payload.Substring(120, 9);
            //            Console.WriteLine(payload + " //// " + azimuth + " //// " + elevation + " //// " + distance);
            //            //Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            //            return Task.CompletedTask;
            //        }
            //        return Task.CompletedTask;
            //    };

            //    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            //    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            //        .WithTopicFilter(f =>f.WithTopic(MqttConfig.Topic))
            //    .Build();

            //    var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            //    Console.WriteLine("MQTT client subscribed.");

            //    //response.DumpToConsole();
            //    Dumper.Dump(response);
            //    Console.ReadLine();
            //}
        }
    }
}
