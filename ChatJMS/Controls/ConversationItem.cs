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
            if (_conversation is GroupConversation)
            {
                lblUsername.Text = ((GroupConversation) c).GetGroupName();
            }
            else
            {
                lblUsername.Text = ((PersonalConversation) c).GetAuthor();
            }
        }

        private void ConversationItem_MouseClick(object sender, MouseEventArgs e)
        {
            ClickItem?.Invoke(_conversation);
        }
    }
}
