using System;
using System.Windows.Forms;
using ChatJMS.Controls;
using ChatJMS.Models;

namespace ChatJMS
{
    public delegate void ClickConItem(Conversation c);
    public delegate void ChatlistDelegate();

    public partial class ChatForm : Form
    {
        private readonly JmsConnection _connection;
        private const string Username = "Jeroen";

        private readonly ClickConItem _clickConItem;

        public ChatForm()
        {
            InitializeComponent();
            Width = 240;
            _connection = new JmsConnection(Username);
            _clickConItem = OpenConversation;
            _connection.Chatlist += UpdateChatList;
        }

        private void OpenConversation(Conversation c)
        {
            _connection.ActiveConversation = c;
            if (Width != 834)
                FormTransform.TransformSize(this, 834, Height);
        }

        delegate void UpdateChatListDelegate();

        private void UpdateChatList()
        {
            if (flpConversations.InvokeRequired)
            {
                var d = new UpdateChatListDelegate(UpdateChatList);
                Invoke(d);
                return;
            }
            flpConversations.Controls.Clear();
            foreach (var con in _connection.GetConversations())
            {
                var c = new ConversationItem(con);
                c.ClickItem += _clickConItem;
                flpConversations.Controls.Add(c);
            }
        }

        private void btnPersonalCon_Click(object sender, EventArgs e)
        {
            var text = tbPersonalCon.Text;
            if (text.Equals(""))
                return;
            var destination = "/queue/" + text;
            _connection.SubscribeTo(destination);
            tbPersonalCon.Text = "";
            var c = new PersonalConversation(_connection.GetDestination(destination), text);
            _connection.AddConversation(c);
            OpenConversation(c);
        }

        private void btnGroupCon_Click(object sender, EventArgs e)
        {
            var text = tbGroupCon.Text;
            if (text.Equals(""))
                return;
            var destination = "/topic/" + text;
            _connection.SubscribeTo(destination);
            tbGroupCon.Text = "";
            var c = new GroupConversation(_connection.GetDestination(destination), text);
            _connection.AddConversation(c);
            OpenConversation(c);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            var messageText = tbMessage.Text;
            if (messageText.Equals(""))
                return;
            if (!_connection.SendMessage(messageText, _connection.ActiveConversation.GetDestination())) return;
            var c = new ChatMessage(Username, DateTime.Now, tbMessage.Text);
            _connection.ActiveConversation.AddMessage(c);
            tbMessage.Text = "";
        }
    }
}
