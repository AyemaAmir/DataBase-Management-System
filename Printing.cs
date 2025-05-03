using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Diagnostics;

namespace BOOKSWOOKS_PROJECT
{   
    public partial class Printing : Form
    {
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
        public Printing()
        {
            InitializeComponent();
            this.Load += new EventHandler(Printing_Load); // Subscribe to the Load event
            FormClosing += Printing_LoadClosing;

        }
        private void Printing_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Printing_Load(object sender, EventArgs e)
        { // Populate DataGridView on form load
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            string query = "SELECT Product_ID, Product_Name, Brand, Description, Price, Cost,  Qty, Items_per_unit,Profit FROM Product WHERE Category_ID = 5";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
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

        private void sidebar1Timer_Tick(object sender, EventArgs e)
        {// Set the minimum and maximum size of sidepanel

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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
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
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomeForm = new outcomes();
            outcomeForm.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users ManageUsersForm = new Manage_Users();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT Product_ID, Product_Name, Brand, Description, Price, Cost, Qty, Items_per_unit,Profit FROM Product WHERE Product_Name Like'" + this.searchTextBox.Text + "%'";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void buttondashboard_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
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
            outcomes outcomeForm = new outcomes();
            outcomeForm.Show();
            this.Hide();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Manage_Users ManageUsersForm = new Manage_Users();
            ManageUsersForm.Show();
            this.Hide();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuButton_Click_1(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DASHBOARD dASHBOARDForm= new DASHBOARD();
            dASHBOARDForm.Show();
            this.Hide();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Invoice invoiceForm = new Invoice();
            invoiceForm.Show();
            this.Hide();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            CreditCustomers creditCustomersForm = new CreditCustomers();
            creditCustomersForm.Show();
            this.Hide();
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
            this.Hide();
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show(); this.Hide();

        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            Manage_Users manage_UsersForm = new Manage_Users();
            manage_UsersForm.Show();
            this.Hide();
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void buttondashboard_Click_1(object sender, EventArgs e)
        {
            DASHBOARD dASHBOARDForm = new DASHBOARD();
            dASHBOARDForm.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Invoice invoiceForm = new Invoice();
                invoiceForm.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CreditCustomers creditCustomersForm = new CreditCustomers();
            creditCustomersForm.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
            this.Hide();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            outcomes outcomesForm=new outcomes();
            outcomesForm.Show();
            this.Hide();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Manage_Users manage_UsersForm = new Manage_Users();
            manage_UsersForm.Show();
            this.Hide();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();

        }
    }
    }

