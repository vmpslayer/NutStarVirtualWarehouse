using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using MQTTnet;
using MQTTnet.Client;


public class MQTTConnection
{
	// MqttFactory mqttFactory;
	private IMqttClient mqttClient;

	public async Task connect()
	{
		Console.WriteLine("Connecting with " + MqttConfig.Server + " and " + MqttConfig.Topic);
		var mqttFactory = new MqttFactory();

		using (this.mqttClient = mqttFactory.CreateMqttClient())
		{
			var mqttClientOptions = new MqttClientOptionsBuilder()
				.WithTcpServer(MqttConfig.Server, MqttConfig.Port)
				//.WithCredentials(MqttConfig.User, MqttConfig.Password)
				.Build();

			mqttClient.ApplicationMessageReceivedAsync += e =>
			{
				Console.WriteLine("Received application message");
                while (e != null)
				{ 
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    return Task.CompletedTask;
                }
                return Task.CompletedTask;
            };

			await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

			var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
				.WithTopicFilter(f =>
				{
					f.WithTopic(MqttConfig.Topic);
				})
				.Build();

			var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

			Console.WriteLine("MQTT client subscribed.");

			//response.DumpToConsole();
			Dumper.Dump(response);

		}
	}

	public async Task disconnect()
	{
		var mqttFactory = new MqttFactory();
		var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();

		await this.mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);

		Console.WriteLine("Disconnect Successful");
	}
};



static class MqttConfig
{
	// Later implementation for multiple topics/tags maybe..
    public static readonly string Server = "192.168.1.2";
    public static readonly int Port = 1883;
	public static readonly string User = "root";
	public static readonly string Password = "12345";
	public static readonly string Topic = "silabs/aoa/angle/ble-pd-4C5BB31129AD/ble-pd-040D84A65F85";
	public static readonly string Topic2 = ""; 
	public static readonly string Topic3 = "";
};

public static class Dumper
{
    public static void Dump(this object obj)
    {
		Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
    }
};


public class Payload
{
    public DateTime? Time { get; set; }
    public String Row { get; set; }
    public String Column { get; set; }
    public String Loft { get; set; }
}

public static class CSVWriter
{
    public static void writeToCSV(Payload payload)
    {
        new Payload { Time = payload.Time, Row = payload.Row, Column = payload.Column, Loft = payload.Loft };
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false,
        };
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        using (var stream = File.Open(directory + "/data/" + "file.csv", FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecord(payload);
            //csv.WriteRecords(records);
        }
    }
};

