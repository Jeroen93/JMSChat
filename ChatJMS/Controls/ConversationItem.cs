using System.Windows.Forms;
using ChatJMS.Models;

namespace ChatJMS.Controls
{
    public partial class ConversationItem : UserControl
    {
        private readonly Conversation _conversation;
        public event ClickConItem ClickItem;

        public ConversationItem(Conversation c)
        {
            InitializeComponent();
            _conversation = c;
            lblLastMessage.Text = c.GetLastMessage().GetMessage();
            lblDate.Text = c.GetLastInteraction().ToShortTimeString();
            if (_conversation is GroupConversation)
            {
                lblUsername.Text = ((GroupConversation)c).GetGroupName();
            }
            else
            {
                lblUsername.Text = ((PersonalConversation)c).GetAuthor();
            }
        }

        public ConversationItem(ChatMessage m)
        {
            InitializeComponent();
            lblLastMessage.Text = m.GetMessage();
            lblDate.Text = m.GetSendDate().ToShortTimeString();
            lblUsername.Text = m.GetAuthor();
        }

        private void ConversationItem_MouseClick(object sender, MouseEventArgs e)
        {
            if (_conversation == null) return;
            ClickItem?.Invoke(_conversation);
        }
    }
}
