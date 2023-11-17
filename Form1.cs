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

namespace Team_308_VirtualWarehouse
{   // THIS IS A TEST COMMENT
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

               // this.BeginInvoke((MethodInvoker)delegate { this.textBox2.Text = "MQTT client subscribed."; });

                //response.DumpToConsole();
                Dumper.Dump(response);

            }
        }

        // get angle values from data
        // azimuth, elevation, distance
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

        // get position values from data
        // x,y,z
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
            // simply sets Textbox3 to a new string value
            this.BeginInvoke((MethodInvoker)delegate { this.Loft_TextBox.Text = "Received application message"; });
            
            // m is the message recieved from the server and locator
            while (m != null)
            {
                // m.ApplicationMessage.Payload contains actual message sent by mqtt broker
                /*This line of code converts the payload (a byte array) received from the MQTT broker into a
                  readable string using UTF-8 encoding and stores the resulting string in the variable "payload"*/
                string payload = Encoding.UTF8.GetString(m.ApplicationMessage.Payload);
                this.BeginInvoke((MethodInvoker)delegate { this.X_TextBox.Text = payload; });
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

                this.BeginInvoke((MethodInvoker)delegate { this.Y_TextBox.Text = x; });
                this.BeginInvoke((MethodInvoker)delegate { this.Loft_TextBox.Text = y; });
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

                Console.WriteLine(payload + " //// " + x + " //// " + y + " //// " + z);
                // CHANGE THIS BACK THIS IS ONLY FOR SCREENSHOT
                this.BeginInvoke((MethodInvoker)delegate { this.X_TextBox.Text = temp1.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.Y_TextBox.Text = temp2.ToString(); });
                this.BeginInvoke((MethodInvoker)delegate { this.Loft_TextBox.Text = distance; });
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

        private void ButtonChangeTextOnClick(object sender, EventArgs e)
        {
            getData();
        }

        private void X_Label_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, X_Label.ClientRectangle,
                Color.Red, ButtonBorderStyle.Solid);
        }
    }
}
