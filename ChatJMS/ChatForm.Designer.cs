namespace ChatJMS
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUsername = new System.Windows.Forms.Label();
            this.flpConversations = new System.Windows.Forms.FlowLayoutPanel();
            this.tbPersonalCon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPersonalCon = new System.Windows.Forms.Button();
            this.btnGroupCon = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbGroupCon = new System.Windows.Forms.TextBox();
            this.gbConversation = new System.Windows.Forms.GroupBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.flpChat = new System.Windows.Forms.FlowLayoutPanel();
            this.gbConversation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(13, 13);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(97, 24);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // flpConversations
            // 
            this.flpConversations.AutoScroll = true;
            this.flpConversations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpConversations.Location = new System.Drawing.Point(13, 41);
            this.flpConversations.Name = "flpConversations";
            this.flpConversations.Size = new System.Drawing.Size(200, 303);
            this.flpConversations.TabIndex = 1;
            // 
            // tbPersonalCon
            // 
            this.tbPersonalCon.Location = new System.Drawing.Point(12, 371);
            this.tbPersonalCon.Name = "tbPersonalCon";
            this.tbPersonalCon.Size = new System.Drawing.Size(172, 20);
            this.tbPersonalCon.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start a new personal chat";
            // 
            // btnPersonalCon
            // 
            this.btnPersonalCon.Location = new System.Drawing.Point(190, 369);
            this.btnPersonalCon.Name = "btnPersonalCon";
            this.btnPersonalCon.Size = new System.Drawing.Size(23, 23);
            this.btnPersonalCon.TabIndex = 4;
            this.btnPersonalCon.Text = ">";
            this.btnPersonalCon.UseVisualStyleBackColor = true;
            this.btnPersonalCon.Click += new System.EventHandler(this.btnPersonalCon_Click);
            // 
            // btnGroupCon
            // 
            this.btnGroupCon.Location = new System.Drawing.Point(190, 418);
            this.btnGroupCon.Name = "btnGroupCon";
            this.btnGroupCon.Size = new System.Drawing.Size(23, 23);
            this.btnGroupCon.TabIndex = 7;
            this.btnGroupCon.Text = ">";
            this.btnGroupCon.UseVisualStyleBackColor = true;
            this.btnGroupCon.Click += new System.EventHandler(this.btnGroupCon_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 401);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connect to a group chat";
            // 
            // tbGroupCon
            // 
            this.tbGroupCon.Location = new System.Drawing.Point(12, 420);
            this.tbGroupCon.Name = "tbGroupCon";
            this.tbGroupCon.Size = new System.Drawing.Size(172, 20);
            this.tbGroupCon.TabIndex = 5;
            // 
            // gbConversation
            // 
            this.gbConversation.Controls.Add(this.btnSendMessage);
            this.gbConversation.Controls.Add(this.tbMessage);
            this.gbConversation.Controls.Add(this.flpChat);
            this.gbConversation.Location = new System.Drawing.Point(228, 13);
            this.gbConversation.Name = "gbConversation";
            this.gbConversation.Size = new System.Drawing.Size(578, 425);
            this.gbConversation.TabIndex = 8;
            this.gbConversation.TabStop = false;
            this.gbConversation.Text = "groupBox1";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(497, 397);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 2;
            this.btnSendMessage.Text = "Send";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(7, 399);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(484, 20);
            this.tbMessage.TabIndex = 1;
            // 
            // flpChat
            // 
            this.flpChat.AutoScroll = true;
            this.flpChat.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpChat.Location = new System.Drawing.Point(7, 20);
            this.flpChat.Name = "flpChat";
            this.flpChat.Size = new System.Drawing.Size(565, 371);
            this.flpChat.TabIndex = 0;
            this.flpChat.WrapContents = false;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 450);
            this.Controls.Add(this.gbConversation);
            this.Controls.Add(this.btnGroupCon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbGroupCon);
            this.Controls.Add(this.btnPersonalCon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPersonalCon);
            this.Controls.Add(this.flpConversations);
            this.Controls.Add(this.lblUsername);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChatForm";
            this.Text = "Chat with JMS";
            this.gbConversation.ResumeLayout(false);
            this.gbConversation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.FlowLayoutPanel flpConversations;
        private System.Windows.Forms.TextBox tbPersonalCon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPersonalCon;
        private System.Windows.Forms.Button btnGroupCon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbGroupCon;
        private System.Windows.Forms.GroupBox gbConversation;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.FlowLayoutPanel flpChat;
    }
}

