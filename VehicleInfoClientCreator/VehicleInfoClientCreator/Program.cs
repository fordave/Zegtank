using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleInfoClientCreator
{
    class Program
    {
        private static readonly Dictionary<string, object> arguments1 = new Dictionary<string, object> { ["x-max-length"] = 1 };

        private const string vehicleExChangeName = "vehicle";

        private const string message_routionkey = "vehicle.kede.message";
        private const string navigationpath_routingkey = "vehicle.kede.navigationpath";

        private const string pickingTaskRoutingkey = "pickingTaskStatus";

        private static readonly string navigationInfoRoutingKey = "vehicleInfo";

        private static readonly string routinesRoutingkey = "vehicleRoutines";

        static void Main(string[] args)
        {
            Console.WriteLine("send message begin");
            Console.WriteLine(" Press [enter] to exit.");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.ExchangeDeclare(exchange: vehicleExChangeName, type: "topic");
                var token = GetRootToken();
                var tasks = new Task[3];
                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = new Task(new Action<object>(s =>
                    {
                        ProduceMQMessage(channel, token, (int)s);
                    }), i);
                    tasks[i].Start();
                    Task.Delay(100);
                }
                Console.Read();
            }
        }

        private static void ProduceMQMessage(IModel channel, JToken token, int index)
        {
            index++;
            do
            {
                token = GetRandomToken(token);
                SendMessage(channel, vehicleExChangeName, navigationpath_routingkey, token.ToString());
                var random = new Random();
                var x = random.Next(10);
                var y = random.Next(10);
                //var message = GetVehicleRoutinesString("dave" + index, x, y);
                //SendMessage(channel, vehicleExChangeName, routinesRoutingkey, message.ToString());
                for (var count = 7; count > 0; count--)
                {
                   var message = GetTaskCountString("dave" + index);
                    SendMessage(channel, vehicleExChangeName, message_routionkey, message.ToString());
                    Thread.Sleep(500);
                    //message = GetPickingTaskString("dave" + index, "test" + count, $"({x},{y})");
                    //SendMessage(channel, vehicleExChangeName, pickingTaskRoutingkey, message.ToString());
                }
                Thread.Sleep(200);
            } while (true);
        }

        private static void SendMessage(IModel channel, string exChangeName, string routingKey, string message)
        {
            Console.WriteLine("Sent {0}", message);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exChangeName,
                           routingKey: routingKey,
                           basicProperties: null,
                           body: body);
        }

        private static string taskCount = "{\"id\":\"\",\"count\":0,\"type\":0}";

        private static JToken GetTaskCountString(string vehicleId)
        {

            var item1s = new Dictionary<string, int>();
            item1s.Add("1", 1);
            item1s.Add("2", 1);
            item1s.Add("3", 1);
            item1s.Add("4", 1);
            var message = new
            {
                id = vehicleId,
                type = 7,
                batchId = "OB0001",
                itemId = "22",
                items = item1s,
                area = new { x = 1, y = 1 },
                status = 2147483664,
            };
            return JToken.Parse(json: JsonConvert.SerializeObject(message));
        }

        private static string pickingTask = "{\"id\":\"\",\"skuname\":\"\",\"type\":0,\"bin\":\"\"}";

        private static JToken GetPickingTaskString(string id, string itemName, string location)
        {
            var token = JToken.Parse(pickingTask);
            token["id"] = id;
            token["type"] = 6;
            token["skuName"] = itemName;
            token["bin"] = location;
            return token;
        }

        private static string vehicleRoutines = "{\"type\":0,\"id\":\"\",\"area\":{\"x\":0,\"y\":0}}";

        private static JToken GetVehicleRoutinesString(string id, int x, int y)
        {
            var token = JToken.Parse(vehicleRoutines);
            token["id"] = id;
            token["type"] = 1;
            var token1 = token["area"];
            token1["x"] = x;
            token1["y"] = y;
            return token;
        }
        private static JToken GetRootToken()
        {
            var path = "vehicleinfo.json";
            if (File.Exists(path))
            {
                using (StreamReader stringReader = new StreamReader(path))
                {
                    var json = stringReader.ReadToEnd();
                    stringReader.Close();
                    return JToken.Parse(json);
                }
            }
            return null;
        }
        private static JToken GetRandomToken(JToken token)
        {
            var random = new Random();
            var min = -10;
            var max = 10;
            for (var i = 0; i < 3; i++)
            {
                var value = random.Next(min, max);
                token[i]["x"] = value;
                token[i]["y"] = value;
                token[i]["yaw"] = Math.PI - random.Next(0, 10) * Math.PI / 5;
                var paths = token[i]["navigationPath"];
                var x = 0;
                foreach (var item in paths.Children())
                {
                    if (x == 0)
                    {
                        x++;
                        item["x"] = value;
                        item["y"] = value;
                    }
                    else
                    {
                        item["x"] = random.Next(min, max);
                        item["y"] = random.Next(min, max);
                    }

                }
                var info = token[i]["infos"];
                info["battery"] = random.NextDouble();
                info["charging"] = false;
            }
            return token;

        }

    }
}
