﻿namespace NIHEI.SC4Buddy.View.Plugins
{
    partial class ReportPluginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportPluginForm));
            this.reportTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reportTextBox
            // 
            this.reportTextBox.AcceptsReturn = true;
            this.reportTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportTextBox.Location = new System.Drawing.Point(12, 25);
            this.reportTextBox.Multiline = true;
            this.reportTextBox.Name = "reportTextBox";
            this.reportTextBox.Size = new System.Drawing.Size(378, 219);
            this.reportTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please enter your report or concern below. Please keep the report in ENGLISH.";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(315, 250);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // reportButton
            // 
            this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reportButton.Location = new System.Drawing.Point(234, 250);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(75, 23);
            this.reportButton.TabIndex = 3;
            this.reportButton.Text = "File report";
            this.reportButton.UseVisualStyleBackColor = true;
            // 
            // ReportPluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 285);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportPluginForm";
            this.Text = "Report plugin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox reportTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button reportButton;
    }
}