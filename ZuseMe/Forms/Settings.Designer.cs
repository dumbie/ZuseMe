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
            this.lbl_ZuseLengthDefault = new System.Windows.Forms.Label();
            this.txt_ZuseLengthDefault = new System.Windows.Forms.TextBox();
            this.lbl_Seconds = new System.Windows.Forms.Label();
            this.lbl_musicbrainz = new System.Windows.Forms.Label();
            this.lbl_StartZuneStartup = new System.Windows.Forms.Label();
            this.lbl_exitZuneOnExit = new System.Windows.Forms.Label();
            this.lbl_StartLastfmStartup = new System.Windows.Forms.Label();
            this.lbl_exitLastfmOnExit = new System.Windows.Forms.Label();
            this.cb_ZuseUseMusicBrainz = new System.Windows.Forms.CheckBox();
            this.cb_StartZuneStartup = new System.Windows.Forms.CheckBox();
            this.cb_exitZuneOnExit = new System.Windows.Forms.CheckBox();
            this.cb_StartLastfmStartup = new System.Windows.Forms.CheckBox();
            this.cb_exitLastfmOnExit = new System.Windows.Forms.CheckBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.cb_ZuseUseMediaKeys = new System.Windows.Forms.CheckBox();
            this.lbl_ZuseUseMediaKeys = new System.Windows.Forms.Label();
            this.lbl_ZuneFilename = new System.Windows.Forms.Label();
            this.txt_ZuneFilename = new System.Windows.Forms.TextBox();
            this.lbl_ZuneFilenameDesc = new System.Windows.Forms.Label();
            this.lbl_version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Location = new System.Drawing.Point(227, 190);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(62, 23);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // lbl_ZuseLengthDefault
            // 
            this.lbl_ZuseLengthDefault.AutoSize = true;
            this.lbl_ZuseLengthDefault.Location = new System.Drawing.Point(11, 12);
            this.lbl_ZuseLengthDefault.Name = "lbl_ZuseLengthDefault";
            this.lbl_ZuseLengthDefault.Size = new System.Drawing.Size(163, 13);
            this.lbl_ZuseLengthDefault.TabIndex = 1;
            this.lbl_ZuseLengthDefault.Text = "Default unknown track length";
            // 
            // txt_ZuseLengthDefault
            // 
            this.txt_ZuseLengthDefault.Location = new System.Drawing.Point(193, 7);
            this.txt_ZuseLengthDefault.Name = "txt_ZuseLengthDefault";
            this.txt_ZuseLengthDefault.Size = new System.Drawing.Size(36, 22);
            this.txt_ZuseLengthDefault.TabIndex = 2;
            // 
            // lbl_Seconds
            // 
            this.lbl_Seconds.AutoSize = true;
            this.lbl_Seconds.Location = new System.Drawing.Point(232, 12);
            this.lbl_Seconds.Name = "lbl_Seconds";
            this.lbl_Seconds.Size = new System.Drawing.Size(50, 13);
            this.lbl_Seconds.TabIndex = 4;
            this.lbl_Seconds.Text = "Seconds";
            // 
            // lbl_musicbrainz
            // 
            this.lbl_musicbrainz.AutoSize = true;
            this.lbl_musicbrainz.Location = new System.Drawing.Point(11, 33);
            this.lbl_musicbrainz.Name = "lbl_musicbrainz";
            this.lbl_musicbrainz.Size = new System.Drawing.Size(176, 13);
            this.lbl_musicbrainz.TabIndex = 5;
            this.lbl_musicbrainz.Text = "Detect track length (MusicBrainz)";
            // 
            // lbl_StartZuneStartup
            // 
            this.lbl_StartZuneStartup.AutoSize = true;
            this.lbl_StartZuneStartup.Location = new System.Drawing.Point(11, 75);
            this.lbl_StartZuneStartup.Name = "lbl_StartZuneStartup";
            this.lbl_StartZuneStartup.Size = new System.Drawing.Size(160, 13);
            this.lbl_StartZuneStartup.TabIndex = 7;
            this.lbl_StartZuneStartup.Text = "Start Zune on ZuseMe startup";
            // 
            // lbl_exitZuneOnExit
            // 
            this.lbl_exitZuneOnExit.AutoSize = true;
            this.lbl_exitZuneOnExit.Location = new System.Drawing.Point(11, 96);
            this.lbl_exitZuneOnExit.Name = "lbl_exitZuneOnExit";
            this.lbl_exitZuneOnExit.Size = new System.Drawing.Size(135, 13);
            this.lbl_exitZuneOnExit.TabIndex = 8;
            this.lbl_exitZuneOnExit.Text = "Exit Zune on ZuseMe exit";
            // 
            // lbl_StartLastfmStartup
            // 
            this.lbl_StartLastfmStartup.AutoSize = true;
            this.lbl_StartLastfmStartup.Location = new System.Drawing.Point(11, 117);
            this.lbl_StartLastfmStartup.Name = "lbl_StartLastfmStartup";
            this.lbl_StartLastfmStartup.Size = new System.Drawing.Size(170, 13);
            this.lbl_StartLastfmStartup.TabIndex = 9;
            this.lbl_StartLastfmStartup.Text = "Start Last.fm on ZuseMe startup";
            // 
            // lbl_exitLastfmOnExit
            // 
            this.lbl_exitLastfmOnExit.AutoSize = true;
            this.lbl_exitLastfmOnExit.Location = new System.Drawing.Point(11, 138);
            this.lbl_exitLastfmOnExit.Name = "lbl_exitLastfmOnExit";
            this.lbl_exitLastfmOnExit.Size = new System.Drawing.Size(145, 13);
            this.lbl_exitLastfmOnExit.TabIndex = 10;
            this.lbl_exitLastfmOnExit.Text = "Exit Last.fm ZuseMe on exit";
            // 
            // cb_ZuseUseMusicBrainz
            // 
            this.cb_ZuseUseMusicBrainz.AutoSize = true;
            this.cb_ZuseUseMusicBrainz.Location = new System.Drawing.Point(214, 32);
            this.cb_ZuseUseMusicBrainz.Name = "cb_ZuseUseMusicBrainz";
            this.cb_ZuseUseMusicBrainz.Size = new System.Drawing.Size(15, 14);
            this.cb_ZuseUseMusicBrainz.TabIndex = 16;
            this.cb_ZuseUseMusicBrainz.UseVisualStyleBackColor = true;
            // 
            // cb_StartZuneStartup
            // 
            this.cb_StartZuneStartup.AutoSize = true;
            this.cb_StartZuneStartup.Location = new System.Drawing.Point(214, 74);
            this.cb_StartZuneStartup.Name = "cb_StartZuneStartup";
            this.cb_StartZuneStartup.Size = new System.Drawing.Size(15, 14);
            this.cb_StartZuneStartup.TabIndex = 17;
            this.cb_StartZuneStartup.UseVisualStyleBackColor = true;
            // 
            // cb_exitZuneOnExit
            // 
            this.cb_exitZuneOnExit.AutoSize = true;
            this.cb_exitZuneOnExit.Location = new System.Drawing.Point(214, 95);
            this.cb_exitZuneOnExit.Name = "cb_exitZuneOnExit";
            this.cb_exitZuneOnExit.Size = new System.Drawing.Size(15, 14);
            this.cb_exitZuneOnExit.TabIndex = 18;
            this.cb_exitZuneOnExit.UseVisualStyleBackColor = true;
            // 
            // cb_StartLastfmStartup
            // 
            this.cb_StartLastfmStartup.AutoSize = true;
            this.cb_StartLastfmStartup.Location = new System.Drawing.Point(214, 116);
            this.cb_StartLastfmStartup.Name = "cb_StartLastfmStartup";
            this.cb_StartLastfmStartup.Size = new System.Drawing.Size(15, 14);
            this.cb_StartLastfmStartup.TabIndex = 19;
            this.cb_StartLastfmStartup.UseVisualStyleBackColor = true;
            // 
            // cb_exitLastfmOnExit
            // 
            this.cb_exitLastfmOnExit.AutoSize = true;
            this.cb_exitLastfmOnExit.Location = new System.Drawing.Point(214, 137);
            this.cb_exitLastfmOnExit.Name = "cb_exitLastfmOnExit";
            this.cb_exitLastfmOnExit.Size = new System.Drawing.Size(15, 14);
            this.cb_exitLastfmOnExit.TabIndex = 20;
            this.cb_exitLastfmOnExit.UseVisualStyleBackColor = true;
            // 
            // btn_Reset
            // 
            this.btn_Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Reset.Location = new System.Drawing.Point(157, 190);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(62, 23);
            this.btn_Reset.TabIndex = 21;
            this.btn_Reset.Text = "Defaults";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // cb_ZuseUseMediaKeys
            // 
            this.cb_ZuseUseMediaKeys.AutoSize = true;
            this.cb_ZuseUseMediaKeys.Location = new System.Drawing.Point(214, 53);
            this.cb_ZuseUseMediaKeys.Name = "cb_ZuseUseMediaKeys";
            this.cb_ZuseUseMediaKeys.Size = new System.Drawing.Size(15, 14);
            this.cb_ZuseUseMediaKeys.TabIndex = 22;
            this.cb_ZuseUseMediaKeys.UseVisualStyleBackColor = true;
            // 
            // lbl_ZuseUseMediaKeys
            // 
            this.lbl_ZuseUseMediaKeys.AutoSize = true;
            this.lbl_ZuseUseMediaKeys.Location = new System.Drawing.Point(11, 54);
            this.lbl_ZuseUseMediaKeys.Name = "lbl_ZuseUseMediaKeys";
            this.lbl_ZuseUseMediaKeys.Size = new System.Drawing.Size(181, 13);
            this.lbl_ZuseUseMediaKeys.TabIndex = 23;
            this.lbl_ZuseUseMediaKeys.Text = "Enable mediakeys (Pause/Resume)";
            // 
            // lbl_ZuneFilename
            // 
            this.lbl_ZuneFilename.AutoSize = true;
            this.lbl_ZuneFilename.Location = new System.Drawing.Point(11, 159);
            this.lbl_ZuneFilename.Name = "lbl_ZuneFilename";
            this.lbl_ZuneFilename.Size = new System.Drawing.Size(172, 13);
            this.lbl_ZuneFilename.TabIndex = 25;
            this.lbl_ZuneFilename.Text = "Default scrobble location (Drive)";
            // 
            // txt_ZuneFilename
            // 
            this.txt_ZuneFilename.Location = new System.Drawing.Point(207, 156);
            this.txt_ZuneFilename.Name = "txt_ZuneFilename";
            this.txt_ZuneFilename.Size = new System.Drawing.Size(22, 22);
            this.txt_ZuneFilename.TabIndex = 26;
            // 
            // lbl_ZuneFilenameDesc
            // 
            this.lbl_ZuneFilenameDesc.AutoSize = true;
            this.lbl_ZuneFilenameDesc.Location = new System.Drawing.Point(232, 159);
            this.lbl_ZuneFilenameDesc.Name = "lbl_ZuneFilenameDesc";
            this.lbl_ZuneFilenameDesc.Size = new System.Drawing.Size(14, 13);
            this.lbl_ZuneFilenameDesc.TabIndex = 27;
            this.lbl_ZuneFilenameDesc.Text = ":\\";
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Location = new System.Drawing.Point(11, 200);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(46, 13);
            this.lbl_version.TabIndex = 28;
            this.lbl_version.Text = "Version";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(301, 223);
            this.Controls.Add(this.lbl_version);
            this.Controls.Add(this.lbl_ZuneFilenameDesc);
            this.Controls.Add(this.txt_ZuneFilename);
            this.Controls.Add(this.lbl_ZuneFilename);
            this.Controls.Add(this.lbl_ZuseUseMediaKeys);
            this.Controls.Add(this.cb_ZuseUseMediaKeys);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.cb_exitLastfmOnExit);
            this.Controls.Add(this.cb_StartLastfmStartup);
            this.Controls.Add(this.cb_exitZuneOnExit);
            this.Controls.Add(this.cb_StartZuneStartup);
            this.Controls.Add(this.cb_ZuseUseMusicBrainz);
            this.Controls.Add(this.lbl_exitLastfmOnExit);
            this.Controls.Add(this.lbl_StartLastfmStartup);
            this.Controls.Add(this.lbl_exitZuneOnExit);
            this.Controls.Add(this.lbl_StartZuneStartup);
            this.Controls.Add(this.lbl_musicbrainz);
            this.Controls.Add(this.lbl_Seconds);
            this.Controls.Add(this.txt_ZuseLengthDefault);
            this.Controls.Add(this.lbl_ZuseLengthDefault);
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
        private System.Windows.Forms.Label lbl_ZuseLengthDefault;
        private System.Windows.Forms.TextBox txt_ZuseLengthDefault;
        private System.Windows.Forms.Label lbl_Seconds;
        private System.Windows.Forms.Label lbl_musicbrainz;
        private System.Windows.Forms.Label lbl_StartZuneStartup;
        private System.Windows.Forms.Label lbl_exitZuneOnExit;
        private System.Windows.Forms.Label lbl_StartLastfmStartup;
        private System.Windows.Forms.Label lbl_exitLastfmOnExit;
        private System.Windows.Forms.CheckBox cb_ZuseUseMusicBrainz;
        private System.Windows.Forms.CheckBox cb_StartZuneStartup;
        private System.Windows.Forms.CheckBox cb_exitZuneOnExit;
        private System.Windows.Forms.CheckBox cb_StartLastfmStartup;
        private System.Windows.Forms.CheckBox cb_exitLastfmOnExit;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.CheckBox cb_ZuseUseMediaKeys;
        private System.Windows.Forms.Label lbl_ZuseUseMediaKeys;
        private System.Windows.Forms.Label lbl_ZuneFilename;
        private System.Windows.Forms.TextBox txt_ZuneFilename;
        private System.Windows.Forms.Label lbl_ZuneFilenameDesc;
        private System.Windows.Forms.Label lbl_version;
    }
}