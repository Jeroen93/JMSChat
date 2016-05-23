using System;
using System.Text;
using System.Windows.Forms;
using Kaazing.JMS;
using Kaazing.JMS.Stomp;

namespace JmsTranslator
{
    public partial class TranslatorForm : Form, IMessageListener
    {
        private const string Serverlocation = "ws://localhost:8001/jms";
        private readonly ISession _session;

        public TranslatorForm()
        {
            InitializeComponent();
            try
            {
                IConnectionFactory connectionFactory = new StompConnectionFactory(new Uri(Serverlocation));
                var connection = connectionFactory.CreateConnection();
                _session = connection.CreateSession(false, SessionConstants.AUTO_ACKNOWLEDGE);
                connection.Start();
                
                tbTranslator.AppendLine("Connected to " + Serverlocation);

                var destination = _session.CreateQueue("/queue/JmsTranslator");
                var consumer = _session.CreateConsumer(destination);
                consumer.MessageListener = this;
                tbTranslator.AppendLine("Translator is ready");
            }
            catch (Exception e)
            {
                tbTranslator.AppendLine("Could not connect: " + e.Message);
            }
        }

        public void OnMessage(IMessage message)
        {

            var sender = message.JMSReplyTo;
            if (sender == null)
            {
                tbTranslator.AppendLine("Message has no applicant set.");
                return;
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
                tbTranslator.AppendLine("Received an IBytesMessage: " + stringMessage);
                SendMessageBack(stringMessage, sender);
            }
            else if (message is IMapMessage)
            {
                tbTranslator.AppendLine("Received an IMapMessage:");
                var mapMessage = (IMapMessage)message;
                var mapNames = mapMessage.MapNames;
                while (mapNames.MoveNext())
                {
                    var name = mapNames.Current;
                    var obj = mapMessage.GetObject(name);
                    if (obj == null)
                    {
                        //Console.WriteLine(name + ": null");
                    }
                    else if (obj.GetType().IsArray)
                    {
                        //Console.WriteLine(name + ": " + BitConverter.ToString((byte[])obj) + " (byte[])");
                    }
                    else
                    {
                        var type = obj.GetType().ToString();
                        Console.WriteLine(type);
                    }
                }
            }
            else
            {
                tbTranslator.AppendLine("UNKNOWN MESSAGE TYPE");
            }
        }

        private void SendMessageBack(string messageText, IDestination destination)
        {
            var producer = _session.CreateProducer(destination);
            var message = _session.CreateTextMessage(messageText);
            producer.Send(message);
            producer.Close();
        }
    }

    public static class WinFormsExtensions
    {
        public static void AppendLine(this TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
    }
}
