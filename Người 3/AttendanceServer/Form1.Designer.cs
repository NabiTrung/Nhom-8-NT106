namespace AttendanceServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtLog = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            SuspendLayout();
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.Location = new Point(12, 159);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(446, 250);
            txtLog.TabIndex = 3;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(32, 60);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(165, 68);
            btnStart.TabIndex = 4;
            btnStart.Text = "START SERVER";
            btnStart.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(273, 60);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(165, 68);
            btnStop.TabIndex = 5;
            btnStop.Text = "STOP SERVER";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(470, 421);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtLog);
            Name = "Form1";
            Text = "AttendanceServer";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Code khởi tạo Form nếu cần sẽ đặt ở đây
        }

        #endregion

        private TextBox txtLog;
        private Button btnStart;
        private Button btnStop;
    }
}
