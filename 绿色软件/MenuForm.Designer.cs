namespace 绿色软件
{
    partial class MenuForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.button_next = new System.Windows.Forms.Button();
			this.button_refresh = new System.Windows.Forms.Button();
			this.button_last = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(0, -8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(612, 299);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("宋体", 20F);
			this.label5.Location = new System.Drawing.Point(148, 89);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(309, 108);
			this.label5.TabIndex = 17;
			this.label5.Text = "数据加载失败，请检查：\r\n网络连接是否正常\r\n是否被封IP\r\n然后重试";
			this.label5.Visible = false;
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(0, 290);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(612, 18);
			this.hScrollBar1.TabIndex = 2;
			this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
			// 
			// button_next
			// 
			this.button_next.Location = new System.Drawing.Point(407, 308);
			this.button_next.Name = "button_next";
			this.button_next.Size = new System.Drawing.Size(205, 23);
			this.button_next.TabIndex = 17;
			this.button_next.Text = "下一页";
			this.button_next.UseVisualStyleBackColor = true;
			this.button_next.Click += new System.EventHandler(this.button_next_Click);
			// 
			// button_refresh
			// 
			this.button_refresh.Location = new System.Drawing.Point(203, 308);
			this.button_refresh.Name = "button_refresh";
			this.button_refresh.Size = new System.Drawing.Size(205, 23);
			this.button_refresh.TabIndex = 16;
			this.button_refresh.Text = "刷新";
			this.button_refresh.UseVisualStyleBackColor = true;
			this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
			// 
			// button_last
			// 
			this.button_last.Location = new System.Drawing.Point(-1, 308);
			this.button_last.Name = "button_last";
			this.button_last.Size = new System.Drawing.Size(205, 23);
			this.button_last.TabIndex = 15;
			this.button_last.Text = "上一页";
			this.button_last.UseVisualStyleBackColor = true;
			this.button_last.Click += new System.EventHandler(this.button_last_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("宋体", 20F);
			this.label6.Location = new System.Drawing.Point(226, 136);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(152, 27);
			this.label6.TabIndex = 18;
			this.label6.Text = "Loading...";
			this.label6.Visible = false;
			// 
			// MenuForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(611, 330);
			this.Controls.Add(this.button_next);
			this.Controls.Add(this.button_refresh);
			this.Controls.Add(this.button_last);
			this.Controls.Add(this.hScrollBar1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MenuForm";
			this.Text = "Menu";
			this.Load += new System.EventHandler(this.MenuForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.Button button_last;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
	}
}