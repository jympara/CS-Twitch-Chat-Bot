﻿namespace TwitchChatBot
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.F1textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// F1textBox1
			// 
			this.F1textBox1.BackColor = System.Drawing.SystemColors.Window;
			this.F1textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.F1textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.F1textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
			this.F1textBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.F1textBox1.Location = new System.Drawing.Point(0, 0);
			this.F1textBox1.Name = "F1textBox1";
			this.F1textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.F1textBox1.Size = new System.Drawing.Size(665, 27);
			this.F1textBox1.TabIndex = 2;
			this.F1textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.F1textBox1_KeyPress);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(665, 52);
			this.Controls.Add(this.F1textBox1);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Twitch Chat Bot - Console";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer timer1;
		public System.Windows.Forms.TextBox F1textBox1;
	}
}

