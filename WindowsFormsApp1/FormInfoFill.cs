using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TwitchChatBot
{
	public partial class FormInfoFill : Form
	{
		private bool CloseConfirm = false;
		private Label label1;
		private TextBox textBox1;
		private TextBox textBox2;
		private Label label2;
		private TextBox textBox3;
		private Button button1;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private LinkLabel linkLabel1;

		public FormInfoFill()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.textBox1.Text.Trim() == "";
			if (flag)
			{
				this.label4.Visible = true;
			}
			else
			{
				this.label4.Visible = false;
			}
			bool flag2 = this.textBox2.Text.Trim() == "";
			if (flag2)
			{
				this.label5.Visible = true;
			}
			else
			{
				this.label5.Visible = false;
			}
			bool flag3 = this.textBox3.Text.Trim() == "";
			if (flag3)
			{
				this.label6.Visible = true;
			}
			else
			{
				this.label6.Visible = false;
			}
			bool flag4 = !this.label4.Visible && !this.label5.Visible && !this.label6.Visible;
			if (flag4)
			{
				this.CloseConfirm = true;
				File.WriteAllText("BotInfo.txt", string.Format("Master Name = {0}\r\n", this.textBox1.Text.Trim()) + string.Format("Bot Name = {0}\r\n", this.textBox2.Text.Trim()) + string.Format("Bot OAuthPassword = {0}", this.textBox3.Text.Trim()));
				base.Close();
			}
		}

		private void FormInfoFill_FormClosing(object sender, FormClosingEventArgs e)
		{
			bool flag = !this.CloseConfirm;
			if (flag)
			{
				new FormConfirmAppClose().ShowDialog();
				e.Cancel = true;
			}
		}

	}
}
