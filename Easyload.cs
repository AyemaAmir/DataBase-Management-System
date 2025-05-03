using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOOKSWOOKS_PROJECT
{
    public partial class Easyload : Form
    {
        bool sidebarExpand;
        public Easyload()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Product ProductForm = new Product();
            ProductForm.Show();
            this.Hide();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Categories CategoriesForm=new Categories();
            CategoriesForm.Show();
               this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
         
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomeForm = new outcomes();
                outcomeForm.Show(); 
                this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users  ManageUsersForm= new Manage_Users();  
                        ManageUsersForm.Show();
                    this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();

        }

        private void sidebar1Timer_Tick(object sender, EventArgs e)
        {//Set the minimum and maximum size of sidepanel

            if (sidebarExpand)
            {
                //if sidebar is expand, minimize
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebar1Timer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebar1Timer.Stop();
                }
            }

        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
