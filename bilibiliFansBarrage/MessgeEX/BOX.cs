using EASkins.Controls;
using System;

namespace bilibiliFansBarrage.MessageEX
{
    public partial class BOX : MaterialForm
    {
        public BOX()
        {
            InitializeComponent();
        }

        public void Showw(string t, string m)
        {
            this.Text = t;
            this.ami_RichTextBox1.Text = m;
            this.ShowDialog();
        }

        private void ami_Button_11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
