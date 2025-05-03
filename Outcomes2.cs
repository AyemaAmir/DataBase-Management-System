using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace BOOKSWOOKS_PROJECT
{
    public partial class Outcomes2 : Form
    {
        public static List<Tuple<int, string>> customerList = new List<Tuple<int, string>>();
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
        bool sidebarExpand;
        public Outcomes2()
        {
            InitializeComponent();
            this.Load += new EventHandler(Outcomes2_Load); // Subscribe to the Load event
            FormClosing += Outcomes2_LoadClosing;

        }
        private void Outcomes2_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Outcomes2_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open(); // Open the connection
                SqlCommand cmd = new SqlCommand("SELECT Cutomer_ID, Customer_Name FROM Customer", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable Customer = new DataTable();
                da.Fill(Customer);

                // Clear the global list before adding new items
                customerList.Clear();

                // Loop through the rows and add the pairs to the list
                foreach (DataRow row in Customer.Rows)
                {
                    int customerID = Convert.ToInt32(row["Cutomer_ID"]);
                    string customerName = row["Customer_Name"].ToString();
                    customerList.Add(new Tuple<int, string>(customerID, customerName));
                }

                // Set the DataSource of comboBox1 to display Customer_Name
                comboBox2.DataSource = Customer;
                comboBox2.DisplayMember = "Customer_Name";
                comboBox2.ValueMember = "Cutomer_ID"; // Optional, if you need the ID
            }
            catch (SqlException ex)
            {
                // Handle SQL errors (e.g., connection issues, invalid query)
                MessageBox.Show("Error retrieving customer information: " + ex.Message);
            }
            finally
            {
                con.Close(); // Close the connection regardless of exceptions
            }


        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
            this.Hide();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();
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

        private void button1_Click(object sender, EventArgs e)
        {

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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes ReportForm = new outcomes();
            ReportForm.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users manage_UsersForm = new Manage_Users();
            manage_UsersForm.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedCustomer = comboBox2.Text;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                if (!string.IsNullOrEmpty(selectedCustomer))
                {
                    string query = "SELECT Contact, Account_No, Credit FROM Customer WHERE Customer_Name = @Customer_Name";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Customer_Name", selectedCustomer);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Contact.Text = reader["Contact"] != DBNull.Value ? reader["Contact"].ToString() : "None";
                                Accountno.Text = reader["Account_No"] != DBNull.Value ? reader["Account_No"].ToString() : "None";
                                checkBox1.Checked = reader["Credit"] != DBNull.Value ? Convert.ToBoolean(reader["Credit"]) : false;
                            }
                            else
                            {
                                Contact.Text = "None";
                                Accountno.Text = "None";
                                checkBox1.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching CUSTOMER RECORD: " + ex.Message);
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string billno = Billno.Text;

                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(billno))
                {
                    MessageBox.Show("Please enter the bill no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                string query = @"
SELECT Bill.*, Bill_Items.Date_of_Updation
FROM Bill
JOIN Bill_Items ON Bill.Bill_No = Bill_Items.Bill_No
WHERE Bill.Bill_No = @billno";

                int customerID = 0;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@billno", billno);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Billno.Text = reader["Bill_No"].ToString();
                            totalbill.Text = reader["Total_Bill"].ToString();
                            paid.Text = reader["Paid"].ToString();
                            advance.Text = reader["Advance"].ToString();
                            balance.Text = reader["Balance"].ToString();
                            discount.Text = reader["Discount"].ToString();
                            dateTimePicker2.Value = Convert.ToDateTime(reader["Date"]);
                            if (reader["Date_of_Updation"] != DBNull.Value)
                            {
                                dateTimePicker1.Value = Convert.ToDateTime(reader["Date_of_Updation"]);
                            }
                            else
                            {
                                dateTimePicker1.Value = DateTime.Today; // Set today's date if Date_of_Updation is null
                            }
                            // Retrieve customer ID
                            customerID = Convert.ToInt32(reader["Customer_ID"]);
                        }
                        else
                        {
                            MessageBox.Show("Bill not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // Query customer table to get customer name
                string customerQuery = "SELECT Customer_Name FROM Customer WHERE Cutomer_ID = @customerID";
                using (SqlCommand customerCmd = new SqlCommand(customerQuery, con))
                {
                    customerCmd.Parameters.AddWithValue("@customerID", customerID);
                    object customerNameObj = customerCmd.ExecuteScalar();
                    if (customerNameObj != null)
                    {
                        comboBox2.Text = customerNameObj.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Customer not found for the given bill.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching the Bill record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string billno = Billno.Text;

                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(billno))
                {
                    MessageBox.Show("Please enter the bill no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }
                string updateBillQuery = @"
UPDATE Bill
SET 
    Paid = @paid,
    Advance = @advance,
    Balance = @balance,    
    Date = @date
WHERE Bill_No = @billno";
                string updateBillItemsQuery = @"
UPDATE Bill_Items
SET Date_of_Updation = @dateOfUpdation
WHERE Bill_No = @billno";
                using (SqlCommand updateBillCmd = new SqlCommand(updateBillQuery, con))
                {

                    updateBillCmd.Parameters.AddWithValue("@paid", paid.Text);
                    updateBillCmd.Parameters.AddWithValue("@advance", advance.Text);
                    updateBillCmd.Parameters.AddWithValue("@balance", balance.Text);
                    updateBillCmd.Parameters.AddWithValue("@date", dateTimePicker2.Value);
                    updateBillCmd.Parameters.AddWithValue("@billno", billno);
                    updateBillCmd.ExecuteNonQuery();
                }
                using (SqlCommand updateBillItemsCmd = new SqlCommand(updateBillItemsQuery, con))
                {
                    updateBillItemsCmd.Parameters.AddWithValue("@billno", billno);
                    updateBillItemsCmd.Parameters.AddWithValue("@dateOfUpdation", dateTimePicker1.Value);
                    updateBillItemsCmd.ExecuteNonQuery();
                }
                MessageBox.Show("Bill updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the Bill record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
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
            outcomes ReportForm = new outcomes();
            ReportForm.Show();
            this.Hide();

        }

        private void button7_Click(object sender, EventArgs e)
        {

            Manage_Users manage_UsersForm = new Manage_Users();
            manage_UsersForm.Show();
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
