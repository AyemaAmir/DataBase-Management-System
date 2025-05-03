using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Diagnostics;


namespace BOOKSWOOKS_PROJECT
{
    public partial class Regularcust : Form
    {
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

        public Regularcust()
        {
            InitializeComponent();
            this.Load += new EventHandler(Regularcust_Load); // Subscribe to the Load event
            FormClosing += Regularcust_LoadClosing;

        }
        private void Regularcust_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
         
        private void Regularcust_Load(object sender, EventArgs e)
        {
            PopulateDataGridView();

        }

        private void PopulateDataGridView()
        {
            string query1 = @"
 SELECT 
    Customer.*, 
    Bill.Bill_No, 
    Bill.Total_Bill, 
    Bill.Paid, 
    Bill.Advance, 
    Bill.Balance, 
    Bill.Date
FROM 
    Customer
INNER JOIN 
    Bill
ON 
    Customer.Cutomer_ID = Bill.Customer_ID AND Bill.Date BETWEEN @FromDate AND @ToDate";
  

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@FromDate", "2024-05-26");
                        command.Parameters.AddWithValue("@ToDate", DateTime.Now);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();
        }

        private void sidebar1Timer_Tick(object sender, EventArgs e)
        {
            // Set the minimum and maximum size of sidepanel

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

      

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            Outcomes2 CustentryForm = new Outcomes2();
            CustentryForm.Show();
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string query2 = @"
SELECT 
    Customer.*, 
    Bill.Bill_No, 
    Bill.Total_Bill, 
    Bill.Paid, 
    Bill.Advance, 
    Bill.Balance, 
    Bill.Date
FROM 
    Customer
INNER JOIN 
    Bill
ON 
    Customer.Cutomer_ID = Bill.Customer_ID And Customer.Customer_Name  Like'" + this.textBox4.Text + "%' AND Bill.Date BETWEEN @FromDate AND @ToDate";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.Date);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            string query2 = @"
SELECT 
    Customer.*, 
    Bill.Bill_No, 
    Bill.Total_Bill, 
    Bill.Paid, 
    Bill.Advance, 
    Bill.Balance, 
    Bill.Date
FROM 
    Customer
INNER JOIN 
    Bill
ON 
    Customer.Cutomer_ID = Bill.Customer_ID And Customer.Credit = 1";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.Date);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
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

        private void menuButton_Click_1(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();

        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
            this.Hide();
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();
            CategoriesForm.Show();
            this.Hide();

        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            outcomes outcomeForm = new outcomes();
            outcomeForm.Show();
            this.Hide();

        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            Manage_Users ManageUsersForm = new Manage_Users();
            ManageUsersForm.Show();
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
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();
            CategoriesForm.Show();
            this.Hide();

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            outcomes outcomeForm = new outcomes();
            outcomeForm.Show();
            this.Hide();

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Manage_Users ManageUsersForm = new Manage_Users();
            ManageUsersForm.Show();
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
