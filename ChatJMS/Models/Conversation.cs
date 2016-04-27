using System;
using System.Collections.Generic;
using System.Linq;
using Kaazing.JMS;

namespace ChatJMS.Models
{
    public abstract class Conversation
    {
        private readonly List<ChatMessage> _messages;
        private DateTime _lastInteraction;
        private readonly IDestination _destinationQueue;

        protected Conversation(IDestination destinationQueue)
        {
            _messages = new List<ChatMessage>();
            _lastInteraction = DateTime.Now;
            _destinationQueue = destinationQueue;
        }

        protected Conversation(ChatMessage message, IDestination destinationQueue)
        {
            _messages = new List<ChatMessage> {message};
            _lastInteraction = DateTime.Now;
            _destinationQueue = destinationQueue;
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
            _lastInteraction = DateTime.Now;
        }

        public List<ChatMessage> GetMessages()
        {
            return _messages;
        }

        public ChatMessage GetLastMessage()
        {
            return _messages.Last();
        }

        public IDestination GetDestination()
        {
            return _destinationQueue;
        }

        public DateTime GetLastInteraction()
        {
            return _lastInteraction;
        }
    }
}
