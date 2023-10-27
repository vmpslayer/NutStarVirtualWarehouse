using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client;
using System.Globalization;

namespace Team_308_VirtualWarehouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Test event handler for button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeTextOnClick(object sender, EventArgs e)
        {
            getData();
        }
        private async void getData()
        {
            Console.WriteLine("Connecting...");
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions2 = new MqttClientOptionsBuilder()
                    .WithTcpServer(MqttConfig.Server, MqttConfig.Port)
                    //.WithCredentials(MqttConfig.User, MqttConfig.Password)
                    .Build();

                var mqttClientOptions3 = new MqttClientOptionsBuilder()
                    .WithTcpServer(MqttConfig.ServeR, MqttConfig.PorT)
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += this.handleReceivedApplicationMessage;

                await mqttClient.ConnectAsync(mqttClientOptions2, CancellationToken.None);
                await mqttClient.ConnectAsync(mqttClientOptions3, CancellationToken.None);

                var mqttSubscribeOptions2 = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    {
                        f.WithTopic(MqttConfig.Topic);
                    })
                .Build();

                var mqttSubscribeOptions3 = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    {
                        f.WithTopic(MqttConfig.Topic);
                    })
                .Build();

                var response2 = await mqttClient.SubscribeAsync(mqttSubscribeOptions2, CancellationToken.None);

                var response3 = await mqttClient.SubscribeAsync(mqttSubscribeOptions3, CancellationToken.None);

                this.BeginInvoke((MethodInvoker)delegate { this.Text = "MQTT client subscribed."; });

                //response.DumpToConsole();
                Dumper.Dump(response2);
                Dumper.Dump(response3);
            }
        }


        private string[] parseApplicationMessage(string payload)
        {
            string[] result = new string[3];

            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i].ToString() + payload[i+1].ToString() == "x\"")
                {
                    result[0] = payload.Substring(i + 3, 9);
                }
                else if (payload[i].ToString() + payload[i+1].ToString() == "y\"")
                {
                    result[1] = payload.Substring(i + 3, 9);
                }
                else if (payload[i].ToString() + payload[i+1].ToString() == "z\"")
                {
                    result[2] = payload.Substring(i + 3, 9);
                    break;
                }
            }
            return result;
        }

        private string[] parsesApplicationMessage(string payload)
        {
            string[] result = new string[3];

            for (int i = 0; i < payload.Length - 1; i++)
            {
                if (payload[i].ToString() + payload[i + 1].ToString() == "x_")
                {
                    result[0] = payload.Substring(i + 10, 8);
                }
                else if (payload[i].ToString() + payload[i + 1].ToString() == "y_")
                {
                    result[1] = payload.Substring(i + 10, 8);
                }
                else if (payload[i].ToString() + payload[i + 1].ToString() == "z_")
                {
                    result[2] = payload.Substring(i + 10, 8);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Handles the received application message
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Task handleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs m) // m is the Mqtt Application that Receives Event Argument Messages
        {
            //this.BeginInvoke((MethodInvoker)delegate { this.textBox6.Text = "Received application message"; });

            while (m != null)
            {
                string payload = Encoding.UTF8.GetString(m.ApplicationMessage.Payload);
                this.BeginInvoke((MethodInvoker)delegate { this.textBox1.Text = payload; });
                string[] result = new string[3];
                string[] result1 = new string[3];
                result = parseApplicationMessage(payload);
                result1 = parsesApplicationMessage(payload);
                string azimuth = result[0];
                string elevation = result[1];
                string distance = result[2];

                string x = result1[0];
                string y = result1[1];
                string z = result1[2];

                CSVWriter.writeToCSV(new Payload() { Time = DateTime.Now, Row = x, Column = y, Loft = z});

                double temp1 = double.Parse(azimuth, CultureInfo.InvariantCulture.NumberFormat);
                double temp2 = double.Parse(elevation, CultureInfo.InvariantCulture.NumberFormat);

                double x_value = double.Parse(x, CultureInfo.InvariantCulture.NumberFormat);
                double y_value = double.Parse(y, CultureInfo.InvariantCulture.NumberFormat);
                double z_value = double.Parse(z, CultureInfo.InvariantCulture.NumberFormat);

                this.BeginInvoke((MethodInvoker)delegate { this.textBox1.Text = x; });
                this.BeginInvoke((MethodInvoker)delegate { this.textBox2.Text = y; });
                float x1 = 1;
                float y1 = 1;

                if (x != null)
                {
                    x1 = float.Parse(x, CultureInfo.InvariantCulture.NumberFormat);
                }
                if (y != null)
                {
                    y1 = float.Parse(y, CultureInfo.InvariantCulture.NumberFormat);
                }
                //8, 33      //-8, 35
                //7.3, 33   //5, 35

                //7, 32     //3, 33
                //5, 33     //2, 32

                //temp1 = (temp1 - 1.5) / 2;
                //temp2 = (temp2 - 33);


                Console.WriteLine(payload + " //// " + azimuth + " //// " + elevation + " //// " + distance);
                this.BeginInvoke((MethodInvoker)delegate { this.textBox1.Text = temp1.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.textBox2.Text = temp2.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.textBox3.Text = distance; });
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
