namespace AttendanceClientTest
{
    partial class Form1
    {
        
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
            lblServerIP = new Label();
            txtServerIP = new TextBox();
            txtLog = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnCheckIn = new Button();
            SuspendLayout();
            // 
            // lblServerIP
            // 
            lblServerIP.AutoSize = true;
            lblServerIP.Location = new Point(89, 72);
            lblServerIP.Name = "lblServerIP";
            lblServerIP.Size = new Size(55, 15);
            lblServerIP.TabIndex = 0;
            lblServerIP.Text = "Server IP:";
            // 
            // txtServerIP
            // 
            txtServerIP.Location = new Point(150, 69);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(230, 23);
            txtServerIP.TabIndex = 1;
            txtServerIP.Text = "127.0.0.1";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(89, 171);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(291, 239);
            txtLog.TabIndex = 2;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(89, 116);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(93, 23);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "CONNECT";
            btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(188, 116);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(93, 23);
            btnDisconnect.TabIndex = 4;
            btnDisconnect.Text = "DISCONNECT";
            btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnCheckIn
            // 
            btnCheckIn.Location = new Point(287, 116);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Size = new Size(93, 23);
            btnCheckIn.TabIndex = 5;
            btnCheckIn.Text = "CHECK IN";
            btnCheckIn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 422);
            Controls.Add(btnCheckIn);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(txtLog);
            Controls.Add(txtServerIP);
            Controls.Add(lblServerIP);
            Name = "Form1";
            Text = "Attendance Client";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        private Label lblServerIP;
        private TextBox txtServerIP;
        private TextBox txtLog;
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnCheckIn;
    }
}
