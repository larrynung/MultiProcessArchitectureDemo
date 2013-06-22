namespace FakeChrome
{
	partial class MainForm
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
			this.btnAddTab = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.SuspendLayout();
			// 
			// btnAddTab
			// 
			this.btnAddTab.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnAddTab.Location = new System.Drawing.Point(0, 377);
			this.btnAddTab.Name = "btnAddTab";
			this.btnAddTab.Size = new System.Drawing.Size(558, 32);
			this.btnAddTab.TabIndex = 1;
			this.btnAddTab.Text = "Add Tab";
			this.btnAddTab.UseVisualStyleBackColor = true;
			this.btnAddTab.Click += new System.EventHandler(this.btnAddTab_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(558, 377);
			this.tabControl1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(558, 409);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnAddTab);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnAddTab;
		private System.Windows.Forms.TabControl tabControl1;
	}
}

