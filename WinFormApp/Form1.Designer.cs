using Crewing;

namespace WinFormApp;

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
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";

        Button btn_test = new Button();
        btn_test.Text = "PRESS!";
        Controls.Add(btn_test);
        btn_test.Click += on_btn_test_click;

        //void EventHandler btn_test_Click(object sender, string args)
        //{

        //}
    }

    private void on_btn_test_click(object sender, EventArgs e)
    {
        //Console.WriteLine("");
        Testing();
    }

    #endregion
}
