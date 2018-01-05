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
            this.stopPlayButton = new System.Windows.Forms.Button();
            this.editModeCheckBox = new System.Windows.Forms.CheckBox();
            this.timeTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.playfieldPictureBox)).BeginInit();
            this.constantsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
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
            this.constantsGroupBox.Size = new System.Drawing.Size(174, 65);
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
            this.loadReplayButton.Size = new System.Drawing.Size(84, 23);
            this.loadReplayButton.TabIndex = 2;
            this.loadReplayButton.Text = "Load Replay";
            this.loadReplayButton.UseVisualStyleBackColor = true;
            this.loadReplayButton.Click += new System.EventHandler(this.loadReplayButton_Click);
            // 
            // togglePauseButton
            // 
            this.togglePauseButton.Location = new System.Drawing.Point(620, 83);
            this.togglePauseButton.Name = "togglePauseButton";
            this.togglePauseButton.Size = new System.Drawing.Size(84, 23);
            this.togglePauseButton.TabIndex = 3;
            this.togglePauseButton.Text = "Toggle Pause";
            this.togglePauseButton.UseVisualStyleBackColor = true;
            this.togglePauseButton.Click += new System.EventHandler(this.togglePauseButton_Click);
            // 
            // stopPlayButton
            // 
            this.stopPlayButton.Location = new System.Drawing.Point(620, 112);
            this.stopPlayButton.Name = "stopPlayButton";
            this.stopPlayButton.Size = new System.Drawing.Size(84, 23);
            this.stopPlayButton.TabIndex = 4;
            this.stopPlayButton.Text = "Stop Play";
            this.stopPlayButton.UseVisualStyleBackColor = true;
            this.stopPlayButton.Click += new System.EventHandler(this.stopPlayButton_Click);
            // 
            // editModeCheckBox
            // 
            this.editModeCheckBox.AutoSize = true;
            this.editModeCheckBox.Location = new System.Drawing.Point(530, 116);
            this.editModeCheckBox.Name = "editModeCheckBox";
            this.editModeCheckBox.Size = new System.Drawing.Size(74, 17);
            this.editModeCheckBox.TabIndex = 5;
            this.editModeCheckBox.Text = "Edit Mode";
            this.editModeCheckBox.UseVisualStyleBackColor = true;
            this.editModeCheckBox.CheckedChanged += new System.EventHandler(this.editModeCheckBox_CheckedChanged);
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.Location = new System.Drawing.Point(12, 402);
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Size = new System.Drawing.Size(512, 45);
            this.timeTrackBar.TabIndex = 6;
            this.timeTrackBar.Scroll += new System.EventHandler(this.timeTrackBar_Scroll);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 439);
            this.Controls.Add(this.timeTrackBar);
            this.Controls.Add(this.editModeCheckBox);
            this.Controls.Add(this.stopPlayButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox playfieldPictureBox;
        private System.Windows.Forms.GroupBox constantsGroupBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label charPosLabel;
        private System.Windows.Forms.Button loadReplayButton;
        private System.Windows.Forms.Button togglePauseButton;
        private System.Windows.Forms.Button stopPlayButton;
        private System.Windows.Forms.CheckBox editModeCheckBox;
        private System.Windows.Forms.TrackBar timeTrackBar;
    }
}

