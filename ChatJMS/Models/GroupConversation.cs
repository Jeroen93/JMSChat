using System.Collections.Generic;
using Kaazing.JMS;

namespace ChatJMS.Models
{
    internal class GroupConversation : Conversation
    {
        private readonly List<string> _authors;
        private readonly string _groupName; 

        public GroupConversation(IDestination destinationQueue, string groupName) : base(destinationQueue)
        {
            _authors = new List<string>();
            _groupName = groupName;
        }

        public GroupConversation(ChatMessage message, IDestination destinationQueue, string groupName) : base(message, destinationQueue)
        {
            _authors = new List<string> {message.GetAuthor()};
            _groupName = groupName;
        }

        public new void AddMessage(ChatMessage message)
        {
            base.AddMessage(message);
            if (!_authors.Contains(message.GetAuthor()))
            {
                _authors.Add(message.GetAuthor());
            }
        }

        public List<string> GetAuthors()
        {
            return _authors;
        }

        public string GetGroupName()
        {
            return _groupName;
        }
    }
}
