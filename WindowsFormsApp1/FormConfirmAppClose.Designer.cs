namespace TwitchChatBot
{
	partial class FormConfirmAppClose
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
			this.label1 = new System.Windows.Forms.Label();
			this.buttonYes = new System.Windows.Forms.Button();
			this.buttonNo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoEllipsis = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(287, 49);
			this.label1.TabIndex = 0;
			this.label1.Text = "Are you sure you want to close the program?";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonYes
			// 
			this.buttonYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonYes.Location = new System.Drawing.Point(16, 87);
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.Size = new System.Drawing.Size(92, 32);
			this.buttonYes.TabIndex = 1;
			this.buttonYes.Text = "Yes";
			this.buttonYes.UseVisualStyleBackColor = true;
			this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
			// 
			// buttonNo
			// 
			this.buttonNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonNo.Location = new System.Drawing.Point(207, 87);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(92, 32);
			this.buttonNo.TabIndex = 2;
			this.buttonNo.Text = "No";
			this.buttonNo.UseVisualStyleBackColor = true;
			this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
			// 
			// FormConfirmAppClose
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(311, 131);
			this.Controls.Add(this.buttonNo);
			this.Controls.Add(this.buttonYes);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.Name = "FormConfirmAppClose";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Close Application";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonYes;
		private System.Windows.Forms.Button buttonNo;
	}
}