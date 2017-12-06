namespace TwitchChatBot
{
	partial class Form2
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


		private System.Windows.Forms.Timer timer2;
		public static System.Windows.Forms.TextBox F2textBox1;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			F2textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 500;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// F2textBox1
			// 
			F2textBox1.BackColor = System.Drawing.SystemColors.Window;
			F2textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			F2textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			F2textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
			F2textBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			F2textBox1.Location = new System.Drawing.Point(0, 0);
			F2textBox1.Multiline = true;
			F2textBox1.Name = "F2textBox1";
			F2textBox1.ReadOnly = true;
			F2textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			F2textBox1.Size = new System.Drawing.Size(518, 602);
			F2textBox1.TabIndex = 2;
			F2textBox1.Text = "Join and Leave events";
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(518, 602);
			this.Controls.Add(F2textBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.Name = "Form2";
			this.Text = "Twitch Chat Bot - Join/Leave";
			this.Load += new System.EventHandler(this.Form2_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

	}
}