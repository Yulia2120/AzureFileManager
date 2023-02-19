using AzureFileManager.Models;
using System;
using System.Windows.Forms;

namespace AzureFileManager
{
    public partial class Form1 : Form
    {
        private FileManager fileManager = new FileManager();
        
        public Form1()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaxiSize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void btnMinSize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
           
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            fileManager.UploadFile();
           
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            foreach (var sItem in fileManager.GetAllFiles())
            {
               listBox1.Items.Add(sItem);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnShowOne_Click(object sender, EventArgs e)
        {
           
            

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedItem = fileManager.GetFileText();
            listBox2.Items.Add(listBox1.SelectedItem);
        }

    }
}
