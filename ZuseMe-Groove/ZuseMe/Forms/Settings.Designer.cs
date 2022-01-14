namespace ZuseMe.Forms
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btn_Save = new System.Windows.Forms.Button();
            this.lbl_TrackLengthDefault = new System.Windows.Forms.Label();
            this.txt_TrackLengthDefault = new System.Windows.Forms.TextBox();
            this.lbl_Seconds = new System.Windows.Forms.Label();
            this.lbl_StartLastfmOnStartup = new System.Windows.Forms.Label();
            this.lbl_CloseLastfmOnExit = new System.Windows.Forms.Label();
            this.cb_StartLastfmOnStartup = new System.Windows.Forms.CheckBox();
            this.cb_CloseLastfmOnExit = new System.Windows.Forms.CheckBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.lbl_version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Location = new System.Drawing.Point(227, 105);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(62, 23);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // lbl_TrackLengthDefault
            // 
            this.lbl_TrackLengthDefault.AutoSize = true;
            this.lbl_TrackLengthDefault.Location = new System.Drawing.Point(11, 12);
            this.lbl_TrackLengthDefault.Name = "lbl_TrackLengthDefault";
            this.lbl_TrackLengthDefault.Size = new System.Drawing.Size(236, 23);
            this.lbl_TrackLengthDefault.TabIndex = 1;
            this.lbl_TrackLengthDefault.Text = "Default unknown track length";
            // 
            // txt_TrackLengthDefault
            // 
            this.txt_TrackLengthDefault.Location = new System.Drawing.Point(193, 7);
            this.txt_TrackLengthDefault.Name = "txt_TrackLengthDefault";
            this.txt_TrackLengthDefault.Size = new System.Drawing.Size(36, 29);
            this.txt_TrackLengthDefault.TabIndex = 2;
            // 
            // lbl_Seconds
            // 
            this.lbl_Seconds.AutoSize = true;
            this.lbl_Seconds.Location = new System.Drawing.Point(232, 12);
            this.lbl_Seconds.Name = "lbl_Seconds";
            this.lbl_Seconds.Size = new System.Drawing.Size(73, 23);
            this.lbl_Seconds.TabIndex = 4;
            this.lbl_Seconds.Text = "Seconds";
            // 
            // lbl_StartLastfmOnStartup
            // 
            this.lbl_StartLastfmOnStartup.AutoSize = true;
            this.lbl_StartLastfmOnStartup.Location = new System.Drawing.Point(11, 36);
            this.lbl_StartLastfmOnStartup.Name = "lbl_StartLastfmOnStartup";
            this.lbl_StartLastfmOnStartup.Size = new System.Drawing.Size(253, 23);
            this.lbl_StartLastfmOnStartup.TabIndex = 9;
            this.lbl_StartLastfmOnStartup.Text = "Start Last.fm on ZuseMe startup";
            // 
            // lbl_CloseLastfmOnExit
            // 
            this.lbl_CloseLastfmOnExit.AutoSize = true;
            this.lbl_CloseLastfmOnExit.Location = new System.Drawing.Point(11, 57);
            this.lbl_CloseLastfmOnExit.Name = "lbl_CloseLastfmOnExit";
            this.lbl_CloseLastfmOnExit.Size = new System.Drawing.Size(218, 23);
            this.lbl_CloseLastfmOnExit.TabIndex = 10;
            this.lbl_CloseLastfmOnExit.Text = "Exit Last.fm on ZuseMe exit";
            // 
            // cb_StartLastfmOnStartup
            // 
            this.cb_StartLastfmOnStartup.AutoSize = true;
            this.cb_StartLastfmOnStartup.Location = new System.Drawing.Point(214, 35);
            this.cb_StartLastfmOnStartup.Name = "cb_StartLastfmOnStartup";
            this.cb_StartLastfmOnStartup.Size = new System.Drawing.Size(22, 21);
            this.cb_StartLastfmOnStartup.TabIndex = 19;
            this.cb_StartLastfmOnStartup.UseVisualStyleBackColor = true;
            // 
            // cb_CloseLastfmOnExit
            // 
            this.cb_CloseLastfmOnExit.AutoSize = true;
            this.cb_CloseLastfmOnExit.Location = new System.Drawing.Point(214, 56);
            this.cb_CloseLastfmOnExit.Name = "cb_CloseLastfmOnExit";
            this.cb_CloseLastfmOnExit.Size = new System.Drawing.Size(22, 21);
            this.cb_CloseLastfmOnExit.TabIndex = 20;
            this.cb_CloseLastfmOnExit.UseVisualStyleBackColor = true;
            // 
            // btn_Reset
            // 
            this.btn_Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Reset.Location = new System.Drawing.Point(157, 105);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(62, 23);
            this.btn_Reset.TabIndex = 21;
            this.btn_Reset.Text = "Defaults";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Location = new System.Drawing.Point(11, 115);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(66, 23);
            this.lbl_version.TabIndex = 28;
            this.lbl_version.Text = "Version";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(301, 139);
            this.Controls.Add(this.lbl_version);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.cb_CloseLastfmOnExit);
            this.Controls.Add(this.cb_StartLastfmOnStartup);
            this.Controls.Add(this.lbl_CloseLastfmOnExit);
            this.Controls.Add(this.lbl_StartLastfmOnStartup);
            this.Controls.Add(this.lbl_Seconds);
            this.Controls.Add(this.txt_TrackLengthDefault);
            this.Controls.Add(this.lbl_TrackLengthDefault);
            this.Controls.Add(this.btn_Save);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "ZuseMe - Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label lbl_TrackLengthDefault;
        private System.Windows.Forms.TextBox txt_TrackLengthDefault;
        private System.Windows.Forms.Label lbl_Seconds;
        private System.Windows.Forms.Label lbl_StartLastfmOnStartup;
        private System.Windows.Forms.Label lbl_CloseLastfmOnExit;
        private System.Windows.Forms.CheckBox cb_StartLastfmOnStartup;
        private System.Windows.Forms.CheckBox cb_CloseLastfmOnExit;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Label lbl_version;
    }
}