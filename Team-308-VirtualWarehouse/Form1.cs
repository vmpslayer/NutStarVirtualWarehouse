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
using System.Drawing.Drawing2D;
using CsvHelper;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Collections.Specialized;
using MQTTnet.Server;

namespace Team_308_VirtualWarehouse
{
    public partial class Form1 : Form
    {
        // Needed for GetCoordinates()
        string[] angleValues = new string[3];
        string[] positionValues = new string[3];
        private bool configured = false;
        private GridMap gridmap;
        int i = 0;

        private IMqttClient mqttClient;
        public Form1()
        {
            InitializeComponent();
            InitializeMqttClient();
            gridmap = new GridMap(this);
        }

        private async void InitializeMqttClient()
        {
            
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClient.ApplicationMessageReceivedAsync += this.handleReceivedApplicationMessage;
            mqttClient.DisconnectedAsync += HandleDisconnected;
            await ConnectAndSubscribeAsync();
            
        }
        private async Task ConnectAndSubscribeAsync()
        {
            var mqttFactory = new MqttFactory();
            var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(MqttConfig.Server, MqttConfig.Port)
            //.WithCredentials(MqttConfig.User, MqttConfig.Password)
            .Build();
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic(MqttConfig.Topic))
            .Build();
            var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            Console.WriteLine("MQTT client subscribed");
            Dumper.Dump(response);
        }
        private Task HandleDisconnected(MqttClientDisconnectedEventArgs arg)
        {
            // Implement reconnection logic here
            return Task.CompletedTask;
        }

        //this function establishes a connection to an MQTT server,
        //subscribes to a specified MQTT topic, and listens for incoming messages
        private async void getData()
        {
            Console.WriteLine("Connecting...");

            //MqttFactory() is used to create a MqttClient instance
            var mqttFactory = new MqttFactory();
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                //The MqttClientOptionsBuilder is used to create an MqttClientOptions object
                //that specifies the MQTT SERVER and PORT to connect to
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(MqttConfig.Server, MqttConfig.Port)
                    //.WithCredentials(MqttConfig.User, MqttConfig.Password)
                    .Build();

                //handleReceivedApplicationMessage function is subscribed to the mqttClient's ApplicationMessageReceivedAsync event
                //whenever an MQTT message is received, the handleReceivedApplicationMessage function will be called to handle it
                mqttClient.ApplicationMessageReceivedAsync += this.handleReceivedApplicationMessage;

                //establish a connection to the MQTT server
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                //create an MqttSubscribeOptions object that specifies the MQTT TOPIC to subscribe to
                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    {
                        f.WithTopic(MqttConfig.Topic);
                    })
                .Build();

                //The mqttClient.SubscribeAsync() method is used to SUBSCRIBE to the specified MQTT TOPIC
                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                //response.DumpToConsole();
                Dumper.Dump(response);
            }
        }

        //When the application receives a message from the MQTT broker, this function is executed

       /* When the button is clicked, it starts the application.
        It receives messages from the MQTT broker and prints the received message in the textbox.

        The function parses the received message to get the values of azimuth, elevation, and distance.

        It then calls the parsesApplicationMessage method to get the values of x, y, and z.
        
        It writes these values to a CSV file using the CSVWriter class.
        
        Finally, it updates the text of textbox1, textbox2, and textbox3 with the
        
        values of azimuth, elevation, distance, x, y, and z respectively.*/
        private Task handleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs m)
        {            
            // m is the message recieved from the server and locator
            while (m != null && i % 2  == 0)
            {
                Console.WriteLine("Received application message");
                // m.ApplicationMessage.Payload contains actual message sent by mqtt broker
                /*This line of code converts the payload (a byte array) received from the MQTT broker into a
                  readable string using UTF-8 encoding and stores the resulting string in the variable "payload"*/
                string payload = Encoding.UTF8.GetString(m.ApplicationMessage.Payload);
                angleValues = getAngleValues(payload);
                positionValues = getPosValues(payload);

                string azimuth = angleValues[0];
                string elevation = angleValues[1];
                string distance = angleValues[2];

                string x = positionValues[0];
                string y = positionValues[1];
                string z = positionValues[2];

                CSVWriter.writeToCSV(new Payload() { Time = DateTime.Now, Row = x, Column = y, Loft = z});

                Console.WriteLine(positionValues[0] + " | " + positionValues[1]);

                i++;
                return Task.CompletedTask;
            }
            i++;
            return Task.CompletedTask;
        }

        private void LocationButton_Click(object sender, EventArgs e)
        {
            if (!configured)
            {
                //configure
                //getData();
                location_button.Text = "Get Location";
                gridmap.setOrigin();
                //gridmap.DisplayOrigin();
                configured = true;
            }
            else 
            {
                string loft = gridmap.calculateGrid();

                this.BeginInvoke((MethodInvoker)delegate { this.X_TextBox.Text = positionValues[0]; });
                this.BeginInvoke((MethodInvoker)delegate { this.Y_TextBox.Text = positionValues[1]; });
                this.BeginInvoke((MethodInvoker)delegate { this.Loft_TextBox.Text = loft; });
                //Console.WriteLine(loft);
                Console.WriteLine("X & Y Coordinates printed");
            }
        }

        public (int x, int y) GetCoordinates()
        {
            int x = -1; int y = -1;
            if (positionValues == null || positionValues.Length < 2)
            {
                throw new InvalidOperationException("Coordinates not initialized or invalid.");
            }

            try
            {
                x = int.Parse(positionValues[0]);
                y = int.Parse(positionValues[1]);
            }
            catch (Exception e)
            {
                x = -2; y = -2;
            }

            return (x , y);
        }

        private void MapButtonClick(object sender, EventArgs e)
        {
            gridmap.Show();
        }

        // get angle values from data
        // azimuth, elevation, distance
        private string[] getAngleValues(string payload)
        {
            string[] result = new string[3];

            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i].ToString() + payload[i + 1].ToString() == "x\"")
                {
                    result[0] = payload.Substring(i + 3, 9);
                }
                else if (payload[i].ToString() + payload[i + 1].ToString() == "y\"")
                {
                    result[1] = payload.Substring(i + 3, 9);
                }
                else if (payload[i].ToString() + payload[i + 1].ToString() == "z\"")
                {
                    result[2] = payload.Substring(i + 3, 9);
                    break;
                }
            }
            return result;
        }

        // get position values from data
        // x,y,z
        private string[] getPosValues(string payload)
        {
            string[] result = new string[3];

            for (int i = 0; i < payload.Length - 1; i++)
            {
                if (payload.Substring(i, 1) == "x")
                {
                    result[0] = getWholeNumber(payload, i);
                }
                else if (payload.Substring(i, 1) == "y")
                {
                    result[1] = getWholeNumber(payload, i);
                }
                else if (payload.Substring(i, 1) == "z")
                {
                    //result[2] = getWholeNumber(payload, i);
                    //break;
                }
            }
            return result;
        }

        private string getWholeNumber(string payload, int index)
        {
            string temp = "";
            for (int j = index + 4; payload.Substring(j, 1) != "."; j++)
            { 
                temp += payload.Substring(j, 1);
            }
            return temp;
        }
    }
}
