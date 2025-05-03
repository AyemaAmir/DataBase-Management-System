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
    public partial class DASHBOARD : Form
    {
        bool sidebarExpand;
        public DASHBOARD()
        {
            InitializeComponent();
            this.Load += new EventHandler(DASHBOARD_Load); // Subscribe to the Load event
            FormClosing += DASHBOARD_LoadClosing;

        }
        private void DASHBOARD_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void DASHBOARD_Load(object sender, EventArgs e)
        {

        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();
        }

        private void sidebar1Timer_Tick(object sender, EventArgs e)
        {
            //Set the minimum and maximum size of sidepanel

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

        private void button1_Click(object sender, EventArgs e)
        {

            Stationery StationeryForm = new Stationery();
            StationeryForm.Show();
            this.Hide();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SPORTSITEMS SPORTSITEMSForm = new SPORTSITEMS ();
            SPORTSITEMSForm.Show();
            this.Hide();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Printing PrintingForm = new Printing();
            PrintingForm.Show();
            this.Hide();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Toys ToysForm = new Toys();
            ToysForm.Show();
            this.Hide();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            CreditCustomers CreditCustomersForm = new CreditCustomers();
            CreditCustomersForm.Show();
            this.Hide();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Regularcust RegularCustomersForm = new Regularcust();
            RegularCustomersForm.Show();
            this.Hide();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Mobile MobileForm = new Mobile();
            MobileForm.Show();
            this.Hide();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Easyload EasyloadForm = new Easyload();
            EasyloadForm.Show();
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
           CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users ManageuserForm = new Manage_Users();
            ManageuserForm.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();
            CategoriesForm.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void buttondashboard_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();
            CategoriesForm.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();
            CategoriesForm.Show();
            this.Hide();


        }

        private void button6_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Manage_Users ManageuserForm = new Manage_Users();
            ManageuserForm.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }
    }
}
