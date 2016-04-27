using Kaazing.JMS;

namespace ChatJMS.Models
{
    internal class PersonalConversation : Conversation
    {
        private readonly string _author;

        public PersonalConversation(IDestination destinationQueue, string recipient) : base(destinationQueue)
        {
            _author = recipient;
        }

        public PersonalConversation(ChatMessage message, IDestination destinationQueue) : base(message, destinationQueue)
        {
            _author = message.GetAuthor();
        }

        public string GetAuthor()
        {
            return _author;
        }
    }
}
