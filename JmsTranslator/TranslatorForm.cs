using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ChatJMS;
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

                Output($"Connected to {Serverlocation}");

                var destination = _session.CreateQueue("/queue/JmsTranslator");
                var consumer = _session.CreateConsumer(destination);
                consumer.MessageListener = this;
                Output("Translator is ready");
            }
            catch (Exception e)
            {
                Output("Could not connect: " + e.Message);
            }
        }

        public void OnMessage(IMessage message)
        {

            var sender = message.JMSReplyTo;
            var author = message.GetStringProperty("author");
            if (sender == null)
            {
                Output("Message has no applicant set.");
                return;
            }

            if (message is ITextMessage)
            {
                var msg = (ITextMessage)message;
                SendMessageBack(msg.Text, message.GetStringProperty("author"), sender);
            }
            else if (message is IBytesMessage)
            {
                var msg = (IBytesMessage)message;
                var actual = new byte[(int)msg.BodyLength];
                msg.ReadBytes(actual);
                var stringMessage = Encoding.Default.GetString(actual, 2, actual.Length-2);
                Output($"Received an IBytesMessage: {stringMessage}");
                SendMessageBack("Translated: " + stringMessage, author, sender);
            }
            else if (message is IMapMessage)
            {
                Output("Received an IMapMessage:");
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
                Output("UNKNOWN MESSAGE TYPE");
            }
        }

        private delegate void OutputDelegate(string text);

        private void Output(string text)
        {
            if (tbTranslator.InvokeRequired)
            {
                var od = new OutputDelegate(Output);
                Invoke(od, text);
                return;
            }
            tbTranslator.AppendLine(text);
        }

        private void SendMessageBack(string messageText, string author, IDestination destination)
        {
            var producer = _session.CreateProducer(destination);
            var message = _session.CreateTextMessage(messageText);
            message.SetStringProperty("author", author);
            message.SetBooleanProperty("group", false);
            producer.Send(message);
            producer.Close();
            Output($"Message send back to {destination}");
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            var nc = new Thread(NewClient);
            nc.Start();
        }

        private static void NewClient()
        {
            var frm = new ChatForm();
            frm.ShowDialog();
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
