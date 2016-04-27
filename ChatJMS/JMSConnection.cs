using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ChatJMS.Models;
using Kaazing.JMS;
using Kaazing.JMS.Stomp;

namespace ChatJMS
{
    internal class JmsConnection : IMessageListener
    {
        public event ChatlistDelegate Chatlist;
        private IConnection _connection;
        private ISession _session;
        private IDictionary<string, List<IMessageConsumer>> _consumers;

        private readonly List<Conversation> _conversations = new List<Conversation>();
        public Conversation ActiveConversation;

        private readonly string _username;
        private readonly string _personalQueue;
        private const string Serverlocation = "ws://localhost:8001/jms";
        private const string GlobalBroadcastTopic = "/topic/GLOBAL_BROADCAST";

        public JmsConnection(string username)
        {
            _username = username;
            _personalQueue = "/queue/" + username;
            Connect();
        }

        private void Connect()
        {
            try
            {
                IConnectionFactory connectionFactory = new StompConnectionFactory(new Uri(Serverlocation));
                _connection = connectionFactory.CreateConnection();

                _consumers = new Dictionary<string, List<IMessageConsumer>>();
                _session = _connection.CreateSession(false, SessionConstants.AUTO_ACKNOWLEDGE);
                _connection.Start();

                SubscribeTo(_personalQueue);
                SubscribeTo(GlobalBroadcastTopic);
            }
            catch (Exception)
            {
                _connection?.Close();
            }
        }

        public bool SendMessage(string messageText, IDestination destination)
        {
            try
            {
                var producer = _session.CreateProducer(destination);
                var message = _session.CreateTextMessage(messageText);
                message.SetStringProperty("author", _username);

                if (destination.ToString().Contains("/topic/"))
                {
                    var grp = (GroupConversation) ActiveConversation;
                    message.SetBooleanProperty("group", true);
                    message.SetStringProperty("groupName", grp.GetGroupName());
                }
                else
                {
                    message.SetBooleanProperty("group", false);
                }

                producer.Send(message);
                producer.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void SubscribeTo(string subscription)
        {
            try
            {
                var destination = GetDestination(subscription);
                var consumer = _session.CreateConsumer(destination);
                consumer.MessageListener = this;

                List<IMessageConsumer> consumerList;
                try
                {
                    consumerList = _consumers[subscription];
                }
                catch (KeyNotFoundException)
                {
                    consumerList = new List<IMessageConsumer>();
                }
                consumerList.Add(consumer);
                _consumers.Add(subscription, consumerList);

            }
            catch (Exception e)
            {
                MessageBox.Show(@"Failure when subscribing: " + e.Message);
            }
        }

        public IDestination GetDestination(string destinationName)
        {
            IDestination destination;
            if (destinationName.StartsWith("/topic/"))
            {
                destination = _session.CreateTopic(destinationName);
            }
            else if (destinationName.StartsWith("/queue/"))
            {
                destination = _session.CreateQueue(destinationName);
            }
            else { throw new ArgumentException(); }
            return destination;
        }

        public void OnMessage(IMessage message)
        {
            try
            {
                if (!(message is ITextMessage))
                    return;
                var textMessage = (ITextMessage) message;
                if (textMessage.GetBooleanProperty("group"))
                {
                    HandleOnGroupMessage(textMessage);
                }
                else
                {
                    HandleOnPersonalMessage(textMessage);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void HandleOnGroupMessage(ITextMessage textMessage)
        {
            if (textMessage.GetStringProperty("autor").Equals(_username))
                return;
            foreach (var con in _conversations)
            {
                var grp = (GroupConversation)con;
                if (!grp.GetGroupName().Equals(textMessage.GetStringProperty("groupName"))) continue;
                var cm = new ChatMessage(textMessage.GetStringProperty("author"), DateTime.Now, textMessage.Text);
                grp.AddMessage(cm);
                //TODO: notify user
            }
        }

        private void HandleOnPersonalMessage(ITextMessage textMessage)
        {
            var author = textMessage.GetStringProperty("author");
            var srp = GetPersonalConversationByAuthor(author);
            if (srp == null)
            {
                srp = new PersonalConversation(new ChatMessage(author, DateTime.Now, textMessage.Text),
                    GetDestination("/queue/" + author));
                _conversations.Add(srp);
                //TODO: notify user
                Chatlist?.Invoke();
            }
            else
            {
                if (ActiveConversation == null || !ActiveConversation.Equals(srp))
                {
                    //TODO: notify user
                }
                var cm = new ChatMessage(author, DateTime.Now, textMessage.Text);
                srp.AddMessage(cm);

            }
        }

        private PersonalConversation GetPersonalConversationByAuthor(string author)
        {
            return _conversations.OfType<PersonalConversation>().FirstOrDefault(pc => pc.GetAuthor().Equals(author));
        }

        public List<Conversation> GetConversations()
        {
            return _conversations;
        }

        public void AddConversation(Conversation c)
        {
            _conversations.Add(c);
        }
    }
}
