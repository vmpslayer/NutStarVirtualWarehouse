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

        private string CategorizeElevation(double elevationData) // Not Complete
        {   // Need to find bounds of elevation, can only go so high
            string categorization;
            if (elevationData > 26)
            {
                throw new ArgumentOutOfRangeException(nameof(elevationData));
            }
            else 
            // make a better implementation of abstract classes with their own variables
            // to make a way to convert double into a char or string easy and cheap
            {
                if (elevationData > 0 && elevationData <= 2) categorization = "A";
                else if (elevationData > 2 && elevationData <= 4) categorization = "B";
                else if (elevationData > 4 && elevationData <= 6) categorization = "C";
                else if (elevationData > 6 && elevationData <= 8) categorization = "D";
                else if (elevationData > 8 && elevationData <= 10) categorization = "E";
                else categorization = "F";
            }
            return categorization;
        }

        //private int NormalizeData(int data)
        //{
        //  
        //}

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
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(MqttConfig.Server, MqttConfig.Port)
                    //.WithCredentials(MqttConfig.User, MqttConfig.Password)
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += this.handleReceivedApplicationMessage;

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    {
                        f.WithTopic(MqttConfig.Topic);
                    })
                .Build();

                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                //this.BeginInvoke((MethodInvoker)delegate { this.textBox5.Text = "MQTT client subscribed."; });
                Console.Write("MQTT Client Subscribed.");

                //response.DumpToConsole();
                Dumper.Dump(response);

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
        private Task handleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs m)
        {
            //this.BeginInvoke((MethodInvoker)delegate { this.textBox6.Text = "Received application message"; });
            Console.Write("Received Application Message");

            while (m != null)
            {
                string payload = Encoding.UTF8.GetString(m.ApplicationMessage.Payload);
                //this.BeginInvoke((MethodInvoker)delegate { this.textBox4.Text = payload; });
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

                double azimuthData = double.Parse(azimuth, CultureInfo.InvariantCulture.NumberFormat);
                double elevationData = double.Parse(elevation, CultureInfo.InvariantCulture.NumberFormat);

                //this.BeginInvoke((MethodInvoker)delegate { this.textBox5.Text = x; });
                //this.BeginInvoke((MethodInvoker)delegate { this.textBox6.Text = y; });
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

                // We want to replace azimuth with raw x, y, z values. (x, y) to be exact, z will be implemented later for use (or we can use elevation)

                Console.WriteLine(payload + " //// " + x + " //// " + y + " //// " + z);
                this.BeginInvoke((MethodInvoker)delegate { this.textBox1.Text = x.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.textBox2.Text = y.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.textBox3.Text = CategorizeElevation(elevationData); });
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
