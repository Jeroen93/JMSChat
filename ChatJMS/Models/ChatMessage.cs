using System;

namespace ChatJMS.Models
{
    public class ChatMessage
    {
        private readonly string _author;
        private readonly DateTime _sendDate;
        private readonly string _message;

        public ChatMessage(string author, DateTime sendDate, string message)
        {
            _author = author;
            _sendDate = sendDate;
            _message = message;
        }

        public string GetAuthor()
        {
            return _author;
        }

        public DateTime GetSendDate()
        {
            return _sendDate;
        }

        public string GetMessage()
        {
            return _message;
        }
    }
}
