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
    public partial class Categories : Form
    {
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
        public Categories()
        {
            InitializeComponent();
            this.Load += new EventHandler(Categories_Load); // Subscribe to the Load event
            FormClosing += Categories_LoadClosing;

        }
        private void Categories_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            // Correctly pass the date values
            string query1 = @"
    SELECT 
        Supplier.*, 
        Purchase.Purchase_Date, 
        Purchase.Purchase_ID, 
        Purchase.Qty_purchased,
       
        Purchase.Total_Amount_Rolled,
        Purchase.Product_ID,
        Purchase.Category_ID
    FROM 
        Supplier
    INNER JOIN 
        Purchase
    ON 
        Supplier.Supplier_ID = Purchase.Supplier_ID 
    AND Purchase.Purchase_Date BETWEEN @FromDate AND @ToDate";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@FromDate", "2024-05-26");
                        command.Parameters.AddWithValue("@ToDate", DateTime.Now);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.AutoGenerateColumns = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions here, e.g., log the error or display a user-friendly message
                MessageBox.Show("An error occurred while retrieving data: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions here, e.g., log the error or display a user-friendly message
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();    
                    EntryForm.Show(); 
                    this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Categories CategoriesForm = new Categories();   
                            CategoriesForm.Show();
                    this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
                    outcomesForm.Show();
                        this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Manage_Users ManageUsersForm = new Manage_Users();
            ManageUsersForm.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
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
            string query1 = @"
SELECT 
    Supplier.*, 
    Purchase.Purchase_Date, 
    Purchase.Purchase_ID, 
    Purchase.Qty_purchased,
    
    Purchase.Total_Amount_Rolled,
    Purchase.Product_ID,
    Purchase.Category_ID
FROM 
    Supplier
INNER JOIN 
    Purchase
ON 
    Supplier.Supplier_ID = Purchase.Supplier_ID And Supplier.Supplier_Name  Like'" + this.searchTextBox.Text + "%' AND Purchase.Purchase_Date BETWEEN @FromDate AND @ToDate";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.Date);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoGenerateColumns = true;

                }
            }
        }

        private void Supplieradd_Click(object sender, EventArgs e)
        {
            string query1 = @"
SELECT 
    Supplier.*, 
    Purchase.Purchase_Date, 
    Purchase.Purchase_ID, 
    Purchase.Qty_purchased,
  
    Purchase.Total_Amount_Rolled,
    Purchase.Product_ID,
    Purchase.Category_ID
FROM 
    Supplier
INNER JOIN 
    Purchase
ON 
    Supplier.Supplier_ID = Purchase.Supplier_ID AND Purchase.Purchase_Date BETWEEN @FromDate AND @ToDate";

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.Date);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoGenerateColumns = true;

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
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
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
        {//set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
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
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
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
