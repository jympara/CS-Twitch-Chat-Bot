using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

namespace TwitchChatBot
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			this.InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			bool flag = Forms.tcpClient.Available > 0 || Forms.reader.Peek() >= 0;
			if (flag)
			{
				string Line = Forms.reader.ReadLine();
				this.F1textBox1.AppendText(string.Format("\r\n{0}", Line));
			}
		}

		private void F1textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			bool flag = e.KeyChar == '\r';
			if (flag)
			{
				string Line = this.F1textBox1.Text;
				Program.MainForm.WriteToTextbox(2, string.Format("{0}\r\n", Line));
				Forms.writer.WriteLine(Line);
				this.F1textBox1.Clear();
			}
		}
	}
}
