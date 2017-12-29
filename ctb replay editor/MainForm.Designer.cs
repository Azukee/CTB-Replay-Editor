namespace ctb_replay_editor
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
            this.playfieldPictureBox = new System.Windows.Forms.PictureBox();
            this.constantsGroupBox = new System.Windows.Forms.GroupBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.charPosLabel = new System.Windows.Forms.Label();
            this.loadReplayButton = new System.Windows.Forms.Button();
            this.togglePauseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.playfieldPictureBox)).BeginInit();
            this.constantsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // playfieldPictureBox
            // 
            this.playfieldPictureBox.Location = new System.Drawing.Point(12, 12);
            this.playfieldPictureBox.Name = "playfieldPictureBox";
            this.playfieldPictureBox.Size = new System.Drawing.Size(512, 384);
            this.playfieldPictureBox.TabIndex = 0;
            this.playfieldPictureBox.TabStop = false;
            // 
            // constantsGroupBox
            // 
            this.constantsGroupBox.Controls.Add(this.timeLabel);
            this.constantsGroupBox.Controls.Add(this.charPosLabel);
            this.constantsGroupBox.Location = new System.Drawing.Point(530, 12);
            this.constantsGroupBox.Name = "constantsGroupBox";
            this.constantsGroupBox.Size = new System.Drawing.Size(200, 65);
            this.constantsGroupBox.TabIndex = 1;
            this.constantsGroupBox.TabStop = false;
            this.constantsGroupBox.Text = "Constants";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(31, 40);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(33, 13);
            this.timeLabel.TabIndex = 1;
            this.timeLabel.Text = "Time:";
            // 
            // charPosLabel
            // 
            this.charPosLabel.AutoSize = true;
            this.charPosLabel.Location = new System.Drawing.Point(11, 21);
            this.charPosLabel.Name = "charPosLabel";
            this.charPosLabel.Size = new System.Drawing.Size(56, 13);
            this.charPosLabel.TabIndex = 0;
            this.charPosLabel.Text = "Char Pos: ";
            // 
            // loadReplayButton
            // 
            this.loadReplayButton.Location = new System.Drawing.Point(530, 83);
            this.loadReplayButton.Name = "loadReplayButton";
            this.loadReplayButton.Size = new System.Drawing.Size(75, 23);
            this.loadReplayButton.TabIndex = 2;
            this.loadReplayButton.Text = "Load Replay";
            this.loadReplayButton.UseVisualStyleBackColor = true;
            this.loadReplayButton.Click += new System.EventHandler(this.loadReplayButton_Click);
            // 
            // togglePauseButton
            // 
            this.togglePauseButton.Location = new System.Drawing.Point(646, 83);
            this.togglePauseButton.Name = "togglePauseButton";
            this.togglePauseButton.Size = new System.Drawing.Size(84, 23);
            this.togglePauseButton.TabIndex = 3;
            this.togglePauseButton.Text = "Toggle Pause";
            this.togglePauseButton.UseVisualStyleBackColor = true;
            this.togglePauseButton.Click += new System.EventHandler(this.togglePauseButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 412);
            this.Controls.Add(this.togglePauseButton);
            this.Controls.Add(this.loadReplayButton);
            this.Controls.Add(this.constantsGroupBox);
            this.Controls.Add(this.playfieldPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "whatever this is";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playfieldPictureBox)).EndInit();
            this.constantsGroupBox.ResumeLayout(false);
            this.constantsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox playfieldPictureBox;
        private System.Windows.Forms.GroupBox constantsGroupBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label charPosLabel;
        private System.Windows.Forms.Button loadReplayButton;
        private System.Windows.Forms.Button togglePauseButton;
    }
}

