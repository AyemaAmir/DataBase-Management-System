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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BOOKSWOOKS_PROJECT
{
    public partial class outcomes : Form
    {
        public static List<Tuple<int, string>> categoryList = new List<Tuple<int, string>>();
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

        public outcomes()
        {
            InitializeComponent();
            this.Load += new EventHandler(outcomes_Load); // Subscribe to the Load event
            FormClosing += outcomes_FormClosing;

           
        }
        private void outcomes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void outcomes_Load(object sender, EventArgs e)
        {
           
            try
            {
                con.Open(); // Open the connection
                SqlCommand cmd = new SqlCommand("SELECT Category_ID, Category_Name FROM Category", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable Category = new DataTable();
                da.Fill(Category);

                // Clear the global list before adding new items
                categoryList.Clear();

                // Add the "All" option manually
                categoryList.Add(new Tuple<int, string>(-1, "All"));

                // Loop through the rows and add the pairs to the list
                foreach (DataRow row in Category.Rows)
                {
                    int categoryID = Convert.ToInt32(row["Category_ID"]);
                    string categoryName = row["Category_Name"].ToString();
                    categoryList.Add(new Tuple<int, string>(categoryID, categoryName));
                }

                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = categoryList;

                comboBox1.DataSource = bindingSource;  // Assuming categoryList holds category data
                comboBox1.DisplayMember = "Item2"; // Assuming "Item2" holds category names (Tuple.Item2)
                comboBox1.ValueMember = "Item1"; // Assuming "Item1" holds category IDs (Tuple.Item1)



                // Now you have a list of tuples containing Category_ID and Category_Name
                // You can use the categoryList as needed in your application
            }
            catch (SqlException ex)
            {
                // Handle SQL errors (e.g., connection issues, invalid query)
                MessageBox.Show("Error retrieving category information: " + ex.Message);
            }
            finally
            {
                con.Close(); // Close the connection regardless of exceptions
            }

        }
        private void PopulateDataGridView(DateTime fromDate, DateTime toDate)
        {

            string query = @"
           SELECT 
    Product.Product_ID,
    Product.Product_Name,
    Product.Price,
    Product.Cost,
    Product.Category_ID,
    Bill_Items.Qty_Saled,
    Bill_Items.profit,
    Bill_Items.Date_of_creation AS Date,
    Bill_Items.Bill_ID,
    Bill_Items.Bill_No
FROM 
    Product
LEFT JOIN 
    Bill_Items ON Product.Product_ID = Bill_Items.Product_ID
  WHERE 
            Bill_Items.Date_of_creation ";
            if (fromDate == toDate)
            {
                query += "= @FromDate";
            }
            else
            {
                query += "BETWEEN @FromDate AND @ToDate";
            }

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                   
                    if (fromDate != toDate)
                    {
                        command.Parameters.AddWithValue("@ToDate", toDate);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    CalculateSums(); // Calculate the sums for all data
                }
            }
        }
        private void CalculateSums()
        {
            decimal totalProfit = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                
                if (row.Cells["profit"].Value != DBNull.Value)
                {
                    totalProfit += Convert.ToDecimal(row.Cells["profit"].Value);
                }
            }

            totalprofit.Text = totalProfit.ToString("N2");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
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

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users manage_usersForm = new Manage_Users();
            manage_usersForm.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

          
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void Supplieradd_Click(object sender, EventArgs e)
        {
            string categoryName = comboBox1.Text; // Assuming comboBox1 is your ComboBox control
                                                  // Validate category selection
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int categoryID = (int)comboBox1.SelectedValue; // Get the selected Category_ID
            DateTime fromDate = dateTimePicker1.Value.Date;
            DateTime toDate = dateTimePicker2.Value.Date;

            if (categoryName == "All")
            {
                PopulateDataGridView(fromDate, toDate);
                CalculateSums(); // Calculate the sums for all data
                                 // Show all data if "All" is selected
                return;
            }

            string query = @"
SELECT 
    Product.Product_ID,
    Product.Product_Name,
    Product.Price,
    Product.Cost,
    Product.Category_ID,
   
    Bill_Items.Qty_Saled,
    Bill_Items.profit,
    Bill_Items.Date_of_creation AS Date,
    Bill_Items.Bill_ID,
    Bill_Items.Bill_No
FROM 
    Product
LEFT JOIN 
    Bill_Items ON Product.Product_ID = Bill_Items.Product_ID 
WHERE  
    Product.Category_ID = @CategoryID AND
    Bill_Items.Date_of_creation";

            if (fromDate == toDate)
            {
                query += " = @FromDate";
            }
            else
            {
                query += " BETWEEN @FromDate AND @ToDate";
            }

            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    // Set parameters
                    command.Parameters.AddWithValue("@CategoryID", categoryID);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    if (fromDate != toDate)
                    {
                        command.Parameters.AddWithValue("@ToDate", toDate);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    CalculateSums(); // Calculate the sums for all data
                }
            }
        }
        private void CalculateTotalBillSum(DateTime fromDate, DateTime toDate)
        {
            try
            {
                con.Open();

                string query = "SELECT SUM(Total_Bill) FROM Bill WHERE Date";

                if (fromDate.Date == toDate.Date)
                {
                    query += " = @FromDate";
                }
                else
                {
                    query += " BETWEEN @FromDate AND @ToDate";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        textBox5.Text = result.ToString();
                    }
                    else
                    {
                        textBox5.Text = "0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total bill sum: " + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                con.Close();
            }
        }
        private void CalculateGrandTotalSum(DateTime fromDate, DateTime toDate)
        {
            try
            {
                con.Open();

                string query = "SELECT SUM(GrandTotal) FROM Bill WHERE Date";

                if (fromDate.Date == toDate.Date)
                {
                    query += " = @FromDate";
                }
                else
                {
                    query += " BETWEEN @FromDate AND @ToDate";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        textBox7.Text = result.ToString();
                    }
                    else
                    {
                        textBox7.Text = "0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total bill sum: " + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                con.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Assume you have DateTimePickers or some other way to get the fromDate and toDate values
            DateTime fromDate = dateTimePicker1.Value;
            DateTime toDate = dateTimePicker2.Value;

            // Call the function to calculate and display the total bill sum
            CalculateTotalBillSum(fromDate, toDate);
            CalculateGrandTotalSum(fromDate,toDate);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();

        }

        private void buttondashboard_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
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
            Manage_Users manage_usersForm = new Manage_Users();
            manage_usersForm.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();

        }

        private void menuButton_Click_1(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();
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
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
            this.Hide();
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
