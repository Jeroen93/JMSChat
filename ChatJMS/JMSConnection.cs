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
        public event ChatlistDelegate ChatListUpdateDelegate;
        public event ChatMessageDelegate ChatMessageUpdateDelegate;

        public Conversation ActiveConversation;

        private IConnection _connection;
        private ISession _session;
        private IDictionary<string, List<IMessageConsumer>> _consumers;
        private readonly List<Conversation> _conversations = new List<Conversation>();
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

        public void Disconnect()
        {
            _session.Close();
            _connection.Close();
            _connection = null;
        }

        public bool SendMessage(string messageText, IDestination destination)
        {
            try
            {
                var producer = _session.CreateProducer(destination);
                var message = _session.CreateTextMessage(messageText);
                message.SetStringProperty("author", _username);

                if (IsTopic(destination))
                {
                    var grp = (GroupConversation)ActiveConversation;
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
                MessageBox.Show(@"Error in SendMessage " + e.Message);
                return false;
            }
        }

        private static bool IsTopic(IDestination destination)
        {
            try
            {
                return (ITopic)destination != null;
            }
            catch (Exception)
            {
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
                {
                    Translate((IBytesMessage)message);
                    return;
                }
                var textMessage = (ITextMessage)message;
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
                MessageBox.Show(@"Error in OnMessage " + e.Message);
            }
        }

        private void HandleOnGroupMessage(ITextMessage textMessage)
        {
            var groupname = textMessage.GetStringProperty("groupName");
            var author = textMessage.GetStringProperty("author");
            if (author.Equals(_username)) return;
            var grp = GetGroupConversationByGroupname(groupname);
            var cm = new ChatMessage(author, DateTime.Now, textMessage.Text);
            grp.AddMessage(cm);
            UpdateScreen(grp);
        }

        private void HandleOnPersonalMessage(ITextMessage textMessage)
        {
            var author = textMessage.GetStringProperty("author");
            if (author.Equals(_username)) return;
            var srp = GetPersonalConversationByAuthor(author);
            if (srp == null)
            {
                srp = new PersonalConversation(new ChatMessage(author, DateTime.Now, textMessage.Text),
                    GetDestination("/queue/" + author));
                _conversations.Add(srp);
            }
            var cm = new ChatMessage(author, DateTime.Now, textMessage.Text);
            srp.AddMessage(cm);
            UpdateScreen(srp);
        }

        private void Translate(IBytesMessage received)
        {
            try
            {
                var bytes = new byte[(int)received.BodyLength];
                received.ReadBytes(bytes);
                var messageToSend = _session.CreateBytesMessage();
                messageToSend.WriteBytes(bytes);
                messageToSend.SetStringProperty("author", received.GetStringProperty("author"));
                messageToSend.SetStringProperty("group", received.GetStringProperty("group"));
                messageToSend.JMSReplyTo = GetDestination(_personalQueue);
                var producer = _session.CreateProducer(GetDestination("/queue/JmsTranslator"));
                producer.Send(messageToSend);
                producer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error in Translate " + e.Message);
            }
        }

        private PersonalConversation GetPersonalConversationByAuthor(string author)
        {
            return _conversations.OfType<PersonalConversation>().FirstOrDefault(pc => pc.GetAuthor().Equals(author));
        }

        private GroupConversation GetGroupConversationByGroupname(string groupname)
        {
            return _conversations.OfType<GroupConversation>().FirstOrDefault(gc => gc.GetGroupName().Equals(groupname));
        }

        private void UpdateScreen(Conversation c)
        {
            if (ActiveConversation != null && ActiveConversation.Equals(c))
            {
                ChatMessageUpdateDelegate?.Invoke();
            }
            ChatListUpdateDelegate?.Invoke();
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
