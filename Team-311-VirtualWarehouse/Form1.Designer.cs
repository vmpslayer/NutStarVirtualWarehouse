namespace Team_308_VirtualWarehouse
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.location_button = new System.Windows.Forms.Button();
            this.X_TextBox = new System.Windows.Forms.TextBox();
            this.Y_TextBox = new System.Windows.Forms.TextBox();
            this.Y_Label = new System.Windows.Forms.Label();
            this.Loft_TextBox = new System.Windows.Forms.TextBox();
            this.Loft_Label = new System.Windows.Forms.Label();
            this.X_Label = new System.Windows.Forms.Label();
            this.MapButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // location_button
            // 
            this.location_button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.location_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(176)))), ((int)(((byte)(0)))));
            this.location_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(120)))), ((int)(((byte)(78)))));
            this.location_button.FlatAppearance.BorderSize = 0;
            this.location_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.location_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.location_button.Location = new System.Drawing.Point(225, 420);
            this.location_button.Margin = new System.Windows.Forms.Padding(0);
            this.location_button.Name = "location_button";
            this.location_button.Size = new System.Drawing.Size(150, 50);
            this.location_button.TabIndex = 7;
            this.location_button.Text = "Test Config";
            this.location_button.UseVisualStyleBackColor = false;
            this.location_button.Click += new System.EventHandler(this.LocationButton_Click);
            // 
            // X_TextBox
            // 
            this.X_TextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.X_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.X_TextBox.Location = new System.Drawing.Point(250, 160);
            this.X_TextBox.Margin = new System.Windows.Forms.Padding(0);
            this.X_TextBox.Name = "X_TextBox";
            this.X_TextBox.ReadOnly = true;
            this.X_TextBox.Size = new System.Drawing.Size(200, 36);
            this.X_TextBox.TabIndex = 8;
            // 
            // Y_TextBox
            // 
            this.Y_TextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Y_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Y_TextBox.Location = new System.Drawing.Point(250, 235);
            this.Y_TextBox.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Y_TextBox.Name = "Y_TextBox";
            this.Y_TextBox.ReadOnly = true;
            this.Y_TextBox.Size = new System.Drawing.Size(200, 36);
            this.Y_TextBox.TabIndex = 9;
            // 
            // Y_Label
            // 
            this.Y_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Y_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(176)))), ((int)(((byte)(0)))));
            this.Y_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Y_Label.CausesValidation = false;
            this.Y_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Y_Label.Location = new System.Drawing.Point(150, 225);
            this.Y_Label.Margin = new System.Windows.Forms.Padding(5);
            this.Y_Label.Name = "Y_Label";
            this.Y_Label.Padding = new System.Windows.Forms.Padding(5);
            this.Y_Label.Size = new System.Drawing.Size(60, 50);
            this.Y_Label.TabIndex = 11;
            this.Y_Label.Text = "Y";
            this.Y_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Loft_TextBox
            // 
            this.Loft_TextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Loft_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Loft_TextBox.Location = new System.Drawing.Point(250, 310);
            this.Loft_TextBox.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Loft_TextBox.Name = "Loft_TextBox";
            this.Loft_TextBox.ReadOnly = true;
            this.Loft_TextBox.Size = new System.Drawing.Size(200, 36);
            this.Loft_TextBox.TabIndex = 12;
            // 
            // Loft_Label
            // 
            this.Loft_Label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Loft_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(176)))), ((int)(((byte)(0)))));
            this.Loft_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Loft_Label.CausesValidation = false;
            this.Loft_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Loft_Label.Location = new System.Drawing.Point(150, 300);
            this.Loft_Label.Margin = new System.Windows.Forms.Padding(5);
            this.Loft_Label.Name = "Loft_Label";
            this.Loft_Label.Padding = new System.Windows.Forms.Padding(5);
            this.Loft_Label.Size = new System.Drawing.Size(60, 50);
            this.Loft_Label.TabIndex = 13;
            this.Loft_Label.Text = "Loft";
            this.Loft_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_Label
            // 
            this.X_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(176)))), ((int)(((byte)(0)))));
            this.X_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.X_Label.CausesValidation = false;
            this.X_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.X_Label.Location = new System.Drawing.Point(150, 150);
            this.X_Label.Margin = new System.Windows.Forms.Padding(5);
            this.X_Label.Name = "X_Label";
            this.X_Label.Padding = new System.Windows.Forms.Padding(5);
            this.X_Label.Size = new System.Drawing.Size(60, 50);
            this.X_Label.TabIndex = 10;
            this.X_Label.Text = "X";
            this.X_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapButton
            // 
            this.MapButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(176)))), ((int)(((byte)(0)))));
            this.MapButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(120)))), ((int)(((byte)(78)))));
            this.MapButton.FlatAppearance.BorderSize = 0;
            this.MapButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.MapButton.Location = new System.Drawing.Point(45, 420);
            this.MapButton.Margin = new System.Windows.Forms.Padding(0);
            this.MapButton.Name = "MapButton";
            this.MapButton.Size = new System.Drawing.Size(150, 50);
            this.MapButton.TabIndex = 14;
            this.MapButton.Text = "Map Button";
            this.MapButton.UseVisualStyleBackColor = false;
            this.MapButton.Click += new System.EventHandler(this.MapButtonClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(574, 529);
            this.Controls.Add(this.MapButton);
            this.Controls.Add(this.Loft_Label);
            this.Controls.Add(this.Loft_TextBox);
            this.Controls.Add(this.Y_Label);
            this.Controls.Add(this.X_Label);
            this.Controls.Add(this.Y_TextBox);
            this.Controls.Add(this.X_TextBox);
            this.Controls.Add(this.location_button);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button location_button;
        private System.Windows.Forms.TextBox Y_TextBox;
        private System.Windows.Forms.Label Y_Label;
        private System.Windows.Forms.TextBox Loft_TextBox;
        private System.Windows.Forms.Label Loft_Label;
        private System.Windows.Forms.Label X_Label;
        private System.Windows.Forms.TextBox X_TextBox;
        private System.Windows.Forms.Button MapButton;
    }
}

