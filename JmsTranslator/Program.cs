using System;
using System.Text;
using Kaazing.JMS;
using Kaazing.JMS.Stomp;

namespace JmsTranslator
{
    internal static class Program
    {
        private static IConnection _connection;
        private static ISession _session;
        private const string Serverlocation = "ws://localhost:8001/jms";

        private static void Main(string[] args)
        {
            try
            {
                IConnectionFactory connectionFactory = new StompConnectionFactory(new Uri(Serverlocation));
                _connection = connectionFactory.CreateConnection();
                _session = _connection.CreateSession(false, SessionConstants.AUTO_ACKNOWLEDGE);
                _connection.Start();
                Console.WriteLine("Connected to " + Serverlocation);

                var destination = _session.CreateQueue("/queue/JmsTranslator");
                var consumer = _session.CreateConsumer(destination);
                consumer.MessageListener = new MessageTranslator(_session);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect: " + e.Message);
            }
            Console.ReadLine();
        }
    }

    internal class MessageTranslator : IMessageListener
    {
        private readonly ISession _session;

        public MessageTranslator(ISession session)
        {
            _session = session;
        }

        public void OnMessage(IMessage message)
        {
            //Console.WriteLine("Hello World");
            var sender = message.GetStringProperty("applicant");
            if (string.IsNullOrWhiteSpace(sender))
            {
                Console.WriteLine("Message has no applicant set");
                //return;
            }

            if (message is ITextMessage)
            {
                var msg = (ITextMessage)message;
                SendMessageBack(msg.Text, sender);
            }
            else if (message is IBytesMessage)
            {
                var msg = (IBytesMessage)message;
                var actual = new byte[(int)msg.BodyLength];
                msg.ReadBytes(actual);
                var stringMessage = Encoding.UTF8.GetString(actual);
                Console.WriteLine("RECEIVED IBytesMessage: " + stringMessage);
                SendMessageBack(stringMessage, sender);
            }
            else if (message is IMapMessage)
            {
                var mapMessage = (IMapMessage)message;
                var mapNames = mapMessage.MapNames;
                while (mapNames.MoveNext())
                {
                    var name = mapNames.Current;
                    var obj = mapMessage.GetObject(name);
                    if (obj == null)
                    {
                        Console.WriteLine(name + ": null");
                    }
                    else if (obj.GetType().IsArray)
                    {
                        Console.WriteLine(name + ": " + BitConverter.ToString((byte[]) obj) + " (byte[])");
                    }
                    else
                    {
                        var type = obj.GetType().ToString();
                        Console.WriteLine(name + ": " + obj + " (" + type + ")");
                    }
                }
                Console.WriteLine("RECEIVED IMapMessage:");
            }
            else
            {
                Console.WriteLine("UNKNOWN MESSAGE TYPE");
            }
        }

        private void SendMessageBack(string messageText, string destination)
        {
            var producer = _session.CreateProducer(_session.CreateQueue(destination));
            var message = _session.CreateTextMessage(messageText);
            producer.Send(message);
            producer.Close();
        }
    }
}
