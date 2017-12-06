using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatBot
{
	public partial class SpamBot : Form
	{
		public bool SpamCycle = false;
		public SpamBot()
		{
			InitializeComponent();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if(SpamCycle == false)
			{

				SpamCycle = true;
			}
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (SpamCycle == false)
			{
				if (e.KeyChar == '\r')
				{

					SpamCycle = true;
				}
			}
		}
	}
}
