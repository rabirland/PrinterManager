﻿namespace UI;

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
        this.blazorWebView1 = new Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView();
        this.SuspendLayout();
        // 
        // blazorWebView1
        // 
        this.blazorWebView1.Dock = DockStyle.Fill;
        this.blazorWebView1.Location = new Point(0, 0);
        this.blazorWebView1.Name = "blazorWebView1";
        this.blazorWebView1.Size = new Size(800, 450);
        this.blazorWebView1.TabIndex = 0;
        this.blazorWebView1.Text = "blazorWebView1";
        this.blazorWebView1.Click += this.blazorWebView1_Click;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 450);
        this.Controls.Add(this.blazorWebView1);
        this.Name = "Form1";
        this.Text = "Form1";
        this.Load += this.Form1_Load;
        this.ResumeLayout(false);
    }

    #endregion

    private Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView blazorWebView1;
}
