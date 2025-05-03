using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BOOKSWOOKS_PROJECT.Form1;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BOOKSWOOKS_PROJECT
{
    public partial class Invoice : Form
    {
       
        public static List<Tuple<int, string>> customerList = new List<Tuple<int, string>>();
        public static List<Tuple<int, string>> productList = new List<Tuple<int, string>>();

        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

        DataTable dt = new DataTable();
        float totalBill = 0.0f;


        public Invoice()
        {

            InitializeComponent();
            // Set the font size of the DataGridView
            // Assuming dataGridView2 is your DataGridView control
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold); // Adjust the font family, size, and style as needed
            dataGridView2.DefaultCellStyle.Font = new Font("Arial", 12);
            this.Load += new EventHandler(Invoice_Load); // Subscribe to the Load event
            FormClosing += Invoice_LoadClosing;
            // Unsubscribe any existing handlers to avoid multiple subscriptions
            dataGridView2.KeyDown -= dataGridView2_KeyDown;
            dataGridView2.KeyDown += dataGridView2_KeyDown;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage); // Add the PrintPage event handler
        }
        private void Invoice_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Invoice_Load(object sender, EventArgs e)
        {
            DisplayNextBillNo();
            // Clear existing columns to avoid duplicate column names error
            dt.Columns.Clear();

            // Define columns for the DataTable
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
     
            dt.Columns.Add("Total Price");
           
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
                comboBox1.DataSource = Customer;
                comboBox1.DisplayMember = "Customer_Name";
                comboBox1.ValueMember = "Cutomer_ID"; // Optional, if you need the ID
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


            try
            {
                con.Open(); // Open the connection
                SqlCommand cmd = new SqlCommand("SELECT Product_ID, Product_Name FROM Product", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable Product = new DataTable();
                da.Fill(Product);

                // Clear the global list before adding new items
               productList.Clear();

                // Loop through the rows and add the pairs to the list
                foreach (DataRow row in Product.Rows)
                {
                    int productID = Convert.ToInt32(row["Product_ID"]);
                    string productName = row["Product_Name"].ToString();
                    customerList.Add(new Tuple<int, string>(productID, productName));
                }

                // Set the DataSource of comboBox1 to display Customer_Name
                comboBox2.DataSource = Product;
                comboBox2.DisplayMember = "Product_Name";
                comboBox2.ValueMember = "Product_ID"; // Optional, if you need the ID
            }
            catch (SqlException ex)
            {
                // Handle SQL errors (e.g., connection issues, invalid query)
                MessageBox.Show("Error retrieving product information: " + ex.Message);
            }
            finally
            {
                con.Close(); // Close the connection regardless of exceptions
            }
        }



            private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private bool IsUserAdmin()
        {
            return UserSession.UserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsUserAdmin())
            {
                DASHBOARD DASHBOARDForm = new DASHBOARD();
                DASHBOARDForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access denied. Only administrators can access the dashboard.");
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {


        }
        private bool isDeleting = false;
        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && !isDeleting)
            {
                isDeleting = true; // Set the flag to true

                try
                {
                    // Perform deletion
                    DialogResult result = MessageBox.Show("Delete key pressed. Do you want to delete the selected row(s)?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                        {
                            if (!row.IsNewRow)
                            {
                                float totalPrice = Convert.ToSingle(row.Cells["Total price"].Value);
                               
                                totalBill -= totalPrice ;

                                Total_Bill.Text = totalBill.ToString();
                                Grand_Total.Text = totalBill.ToString();

                                dataGridView2.Rows.Remove(row);
                            }
                        }
                    }

                    // Suppress further processing of the delete key
                    e.SuppressKeyPress = true;
                }
                finally
                {
                    // Reset the flag after the delete action is completed
                    isDeleting = false;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox2_Click(this, EventArgs.Empty); // Trigger the PictureBox click event handler
                return true; // Indicate that the key press has been handled
            }
            return base.ProcessCmdKey(ref msg, keyData); // Process other key events normally
        }





        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                string productName = comboBox2.Text; // Assuming comboBox1 is your ComboBox control

                if (string.IsNullOrEmpty(productName))
                {
                    MessageBox.Show("Please select a product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if no category is selected
                }

                // Find the Category_ID for the selected Category_Name
                int productID = GetProductIDByProductName(productName); // Default value in case no match is found

                foreach (var tuple in productList)
                {
                    if (tuple.Item2 == productName)
                    {
                        productID = tuple.Item1;
                        break; // Exit the loop once a match is found
                    }
                }

                if (productID == -1)
                {
                    MessageBox.Show("Selected product is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if the category is not found
                }


                // Fetch product details from database
                // Fetch product details from database
                string query = "SELECT Product_Id, Price,Items_per_unit FROM Product WHERE Product_Name = @Product_Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Product_Name", productName);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        float price = Convert.ToSingle(reader["Price"]);
  
                        int quantity = int.TryParse(Qty.Text, out int parsedQuantity) ? parsedQuantity : 0;
                        float units = float.TryParse(Purchased_Units.Text, out float parsedUnits) ? parsedUnits : 0.0f;
                        float itemsPerUnit = Convert.ToSingle(reader["Items_per_unit"]);
                        // Calculate the total price for the current product
                        float totalPrice = units > 0 ? itemsPerUnit * units * price : quantity * price;

                        // Add the current product's total price to the running total
                        totalBill += totalPrice;



                        // Update the Total_Bill.Text textbox with the new running total
                        Total_Bill.Text = totalBill.ToString();
                        // Update the Grand_Total.Text textbox with the new running total
                        Grand_Total.Text = totalBill.ToString();

                        // Determine the quantity to add to the DataTable
                        int displayQuantity = units > 0 ? (int)(itemsPerUnit * units) : quantity;

                        // Add product details to DataTable
                        DataRow row = dt.NewRow();
                        row["Product Name"] = productName;
                        row["Price"] = price;
                        if (units > 0)
                        { row["Quantity"] = displayQuantity; }
                        else { row["Quantity"] = quantity; }
                        
                        row["Total price"] = totalPrice;
                       
                        dt.Rows.Add(row);

                        // Bind the DataTable to the DataGridView
                        dataGridView2.DataSource = dt;

                        // Clear input fields

                        Qty.Text = "0";
                        Purchased_Units.Text = "0.0";
                        discount.Text = "0.00";
                    }
                    else
                    {
                        MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing product details: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private int GetProductIDByProductName(string productName)
        {
            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Product_ID FROM Product WHERE Product_Name = @ProductName";  // Modified to use Product_Name
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ProductName", productName);  // Modified parameter name

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader["Product_ID"]);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        private int GetLatestBillNumber()
        {
            int latestBillNo = 0;
            string query = "SELECT MAX(Bill_No) FROM Bill";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        latestBillNo = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving latest bill number: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return latestBillNo;
        }



        private void button3_Click(object sender, EventArgs e)
        {

            printDialog1 = new PrintDialog();
            printDialog1.Document = printDocument1;
            DialogResult = printDialog1.ShowDialog();
            if (DialogResult == DialogResult.OK) { printDocument1.Print();// Bring the form back to the foreground
                this.BringToFront();
                this.Activate();
            }
            try
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string customerName = comboBox1.Text; // Assuming comboBox1 is your ComboBox control
                        string selectedProduct = comboBox2.Text;
                        // Check if the customer already exists
                        string query = "SELECT Cutomer_ID FROM Customer WHERE Customer_Name = @Customer_Name";
                        int? customerId = null;

                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Customer_Name", customerName);

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                customerId = Convert.ToInt32(result);
                            }
                        }

                        string phoneNo = Contact.Text;
                        string accountNo = Account_No.Text;
                        bool credit = checkBox1.Checked;

                        // If customer does not exist, insert new customer
                        if (!customerId.HasValue)
                        {
                            string insertCustomerQuery = "INSERT INTO Customer (Customer_Name, Contact, Account_No, Credit) VALUES (@CustomerName, @PhoneNo, @AccountNo, @Credit); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand cmdCustomer = new SqlCommand(insertCustomerQuery, con, transaction))
                            {
                                cmdCustomer.Parameters.AddWithValue("@CustomerName", customerName);
                                cmdCustomer.Parameters.AddWithValue("@PhoneNo", phoneNo);
                                cmdCustomer.Parameters.AddWithValue("@AccountNo", accountNo);
                                cmdCustomer.Parameters.AddWithValue("@Credit", credit);

                                customerId = Convert.ToInt32(cmdCustomer.ExecuteScalar());
                            }
                        }


                        string insertBillQuery = "INSERT INTO Bill (Date, Total_Bill,GrandTotal, Paid, Advance, Balance, Discount, Customer_ID) VALUES (@Date, @TotalBill, @GrandTotal,@Paid, @Advance, @Balance, @Discount, @CustomerId); SELECT SCOPE_IDENTITY();";
                        int billno;
                        using (SqlCommand cmdBill = new SqlCommand(insertBillQuery, con, transaction))
                        {
                            cmdBill.Parameters.AddWithValue("@Date", DateTime.Now);
                            cmdBill.Parameters.AddWithValue("@TotalBill", float.TryParse(Total_Bill.Text, out float totalBill) ? totalBill : 0);
                            cmdBill.Parameters.AddWithValue("@Paid", float.TryParse(Paid.Text, out float paid) ? paid : 0);
                            cmdBill.Parameters.AddWithValue("@Advance", float.TryParse(Advance.Text, out float advance) ? advance : 0);
                            cmdBill.Parameters.AddWithValue("@Discount", float.TryParse(discount.Text, out float discountValue) ? discountValue : 0);
                            cmdBill.Parameters.AddWithValue("@Balance", float.TryParse(Balance.Text, out float balance) ? balance : 0);
                            cmdBill.Parameters.AddWithValue("@CustomerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                            cmdBill.Parameters.AddWithValue("@GrandTotal", float.TryParse(Grand_Total.Text, out float grandTotal) ? grandTotal : 0);
                            billno = Convert.ToInt32(cmdBill.ExecuteScalar());
                        }

                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.IsNewRow) continue;

                            float quantity = float.TryParse(row.Cells["Quantity"].Value.ToString(), out float qty) ? qty : 0;
                            float totalPrice = float.TryParse(row.Cells["Total Price"].Value.ToString(), out float prc) ? prc : 0;





                            string Productname = row.Cells["Product Name"].Value.ToString();
                            int productID = GetProductIDByProductName(Productname);

                            if (productID == -1)
                            {
                                MessageBox.Show("Product not found: " + Productname);
                                continue;
                            }




                            string insertBillItemsQuery = "INSERT INTO Bill_Items (Qty_saled, TotalPrice_for_product, Product_ID, Bill_No, Date_of_creation) VALUES (@Qty, @TotalPrice,@ProductID, @Billno, @Date)";
                            using (SqlCommand cmdBillItems = new SqlCommand(insertBillItemsQuery, con, transaction))
                            {
                                cmdBillItems.Parameters.AddWithValue("@Qty", quantity);
                                cmdBillItems.Parameters.AddWithValue("@TotalPrice", totalPrice);

                                cmdBillItems.Parameters.AddWithValue("@ProductID", productID);
                                cmdBillItems.Parameters.AddWithValue("@Billno", billno);
                                cmdBillItems.Parameters.AddWithValue("@Date", DateTime.Now);
                                cmdBillItems.ExecuteNonQuery();
                            }

                            string selectAndUpdateProductQuery = @"
DECLARE @CurrentQuantity INT;
DECLARE @Price DECIMAL(18, 2);  -- Adjust precision and scale as needed
DECLARE @Cost DECIMAL(18, 2);
DECLARE @ProfitPerUnit DECIMAL(18, 2);
DECLARE @TotalProfit DECIMAL(18, 2);
DECLARE @Profit DECIMAL(18, 2);
-- Retrieve price, cost, and quantity
SELECT @Price = Price, @Cost = Cost, @CurrentQuantity = Qty,@Profit=Profit
FROM Product
WHERE Product_ID = @ProductID;

-- Calculate profit per unit and total profit
SET @ProfitPerUnit = @Price - @Cost;
SET @TotalProfit = @CurrentQuantity * @ProfitPerUnit;

-- Update profit and quantity
UPDATE Product
SET Profit =@Profit + @TotalProfit, Qty = @CurrentQuantity - @Quantity
WHERE Product_ID = @ProductID;

-- Insert TotalProfit into Bill_Items table
UPDATE Bill_Items
SET profit = @TotalProfit
WHERE Product_ID = @ProductID AND Bill_No = @Billno;
";



                            using (SqlCommand cmd = new SqlCommand(selectAndUpdateProductQuery, con, transaction))
                            {
                                // Add parameters for ProductID and quantity
                                cmd.Parameters.AddWithValue("@ProductID", productID);
                                cmd.Parameters.AddWithValue("@Quantity", quantity);
                                cmd.Parameters.AddWithValue("@Billno", billno);
                                // Execute the command
                                cmd.ExecuteNonQuery();
                            }


                        }

                        transaction.Commit();
                        MessageBox.Show("Invoice saved successfully.");
                        DisplayNextBillNo();
                        // Reset the totalBill, Grand_Total, and Balance variables
                        totalBill = 0.0f;
                        Total_Bill.Text = "0.00";
                        Grand_Total.Text = "0.00";
                        Balance.Text = "0.00";

                        // Clear the input fields
                        Contact.Text = "";
                        Account_No.Text = "";
                        Paid.Text = "0.00";
                        Advance.Text = "0.00";
                        discount.Text = "0.00";
                        dt.Rows.Clear();
                        dataGridView2.DataSource = null;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error saving invoice: " + ex.Message + "\n" + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                con.Close();


                dataGridView2.DataSource = dt;
            }


        }
        private void Product_Name_TextChanged(object sender, EventArgs e)
        {

        }

        private void discount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Calculate total bill from DataTable by summing total product price and total unit price
                float totalBill = 0.0f;
                foreach (DataRow row in dt.Rows)
                {
                    float totalProductPrice = float.Parse(row["Total price"].ToString());
                 
                    totalBill += totalProductPrice;
                }

                // Update Total_Bill after calculation
                Total_Bill.Text = totalBill.ToString();

                // Calculate the grand total after applying the discount
                if (float.TryParse(discount.Text, out float discountValue))
                {
                    float grandTotal = totalBill * ((100 - discountValue) / 100);
                    Grand_Total.Text = grandTotal.ToString();
                }
                else
                {
                    Grand_Total.Text = totalBill.ToString(); // No discount applied
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating discount: " + ex.Message);
            }
            finally
            {  // Close the connection
                con.Close();
               
            }
        }

        private void Paid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Calculate the grand total after applying the discount
                if (float.TryParse(discount.Text, out float discountValue))
                {
                    float grandTotal = totalBill * ((100 - discountValue) / 100);
                    Grand_Total.Text = grandTotal.ToString();

                    // Update the balance
                    if (float.TryParse(Paid.Text, out float paid))
                    {
                        float balance = paid - grandTotal;
                        Balance.Text = balance.ToString();
                    }
                    else
                    {
                        Balance.Text = grandTotal.ToString(); // No paid amount entered
                    }
                }
                else
                {
                    Grand_Total.Text = totalBill.ToString(); // No discount applied

                    // Update the balance
                    if (float.TryParse(Paid.Text, out float paid))
                    {
                        float balance = totalBill - paid;
                        Balance.Text = balance.ToString();
                    }
                    else
                    {
                        Balance.Text = totalBill.ToString(); // No paid amount entered
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating balance: " + ex.Message);
            }
            finally
            {  // Close the connection
                con.Close();
                
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Selected(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedProduct = comboBox2.Text;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                if (!string.IsNullOrEmpty(selectedProduct))
                {

                    string query = "SELECT Price FROM Product WHERE Product_Name = @Product_Name";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Product_Name", selectedProduct);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox8.Text = reader["Price"] != DBNull.Value ? reader["Price"].ToString() : "None";
                               
                            }
                            else
                            {
                                textBox8.Text = "0";
                               
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching Product price  : " + ex.Message);
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                string selectedCustomer = comboBox1.Text;
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
                                Account_No.Text = reader["Account_No"] != DBNull.Value ? reader["Account_No"].ToString() : "None";
                                checkBox1.Checked = reader["Credit"] != DBNull.Value ? Convert.ToBoolean(reader["Credit"]) : false;
                            }
                            else
                            {
                                Contact.Text = "None";
                                Account_No.Text = "None";
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
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string typedCustomerName = comboBox1.Text;

            // Check if the typed customer name is in the list of customers
            bool customerExists = customerList.Any(c => c.Item2 == typedCustomerName);

            if (!customerExists)
            {
                Contact.Text = "";
                Account_No.Text = "";
                checkBox1.Checked = false;
            }
        }

        private void Purchased_Units_TextChanged(object sender, EventArgs e)
        {

        }



        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 10, FontStyle.Regular);
            Font printFont = new Font("Arial", 8, FontStyle.Regular);
            Font subHeaderFontBold = new Font("Arial", 10, FontStyle.Bold);

            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float yPos = topMargin;
            float fontHeight = printFont.GetHeight(e.Graphics);

            // Print Header
            e.Graphics.DrawString("              BOOKS WOOKS", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print SubHeader
            e.Graphics.DrawString("Address: Shop# 7, Gondal Tower, Haji A.Ghani Road,", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
            e.Graphics.DrawString("Sector H-13, Islamabad  Contact#: 0349-7717345 ", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
           
            e.Graphics.DrawString("     Email: bookswooks7@gmail.com", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print Date, Time, and Bill No
            e.Graphics.DrawString("Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + "    Time: " + DateTime.Now.ToString("hh:mm:ss tt") + "    Bill No: " + textBox18.Text, printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print Separator Line
            e.Graphics.DrawString(new string('-', 80), printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;

            // Print Sales Invoice Header
            e.Graphics.DrawString("              SALES INVOICE", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print Customer Details
            e.Graphics.DrawString("M/s: " + comboBox1.Text, subHeaderFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print Column Headers
            e.Graphics.DrawString("S#", subHeaderFont, Brushes.Black, leftMargin, yPos);
            e.Graphics.DrawString("Item Name", subHeaderFont, Brushes.Black, leftMargin + 30, yPos);
            e.Graphics.DrawString("Qty.", subHeaderFont, Brushes.Black, leftMargin + 150, yPos);
            e.Graphics.DrawString("Rate", subHeaderFont, Brushes.Black, leftMargin + 200, yPos);
            e.Graphics.DrawString("Amount", subHeaderFont, Brushes.Black, leftMargin + 250, yPos);
            yPos += fontHeight*2;

            // Print Items
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dataGridView2.Rows[i];

                string productName = row.Cells["Product Name"].Value.ToString();
                string qty = row.Cells["Quantity"].Value.ToString();
                string price = row.Cells["Price"].Value.ToString();
                string totalPrice = row.Cells["Total Price"].Value.ToString();

                e.Graphics.DrawString((i + 1).ToString(), printFont, Brushes.Black, leftMargin, yPos);
                e.Graphics.DrawString(productName, printFont, Brushes.Black, leftMargin + 30, yPos);
                e.Graphics.DrawString(qty, printFont, Brushes.Black, leftMargin + 150, yPos);
                e.Graphics.DrawString(price, printFont, Brushes.Black, leftMargin + 200, yPos);
                e.Graphics.DrawString(totalPrice, printFont, Brushes.Black, leftMargin + 250, yPos);
                yPos += fontHeight;
            }

            // Print Separator Line
            e.Graphics.DrawString(new string('-', 80), printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight*2;

            // Print Totals
            e.Graphics.DrawString("Total Bill: " + Total_Bill.Text, subHeaderFontBold, Brushes.Black, leftMargin, yPos);
            e.Graphics.DrawString("Balance: " + Balance.Text, subHeaderFont, Brushes.Black, leftMargin + 150, yPos);
            yPos += fontHeight+2;
            e.Graphics.DrawString("Received: " + Paid.Text, subHeaderFont, Brushes.Black, leftMargin, yPos);
            e.Graphics.DrawString("Gross Total: " + Grand_Total.Text, subHeaderFontBold, Brushes.Black, leftMargin + 150, yPos);
            yPos += fontHeight+2;
            e.Graphics.DrawString("Discount: " + discount.Text, subHeaderFont, Brushes.Black, leftMargin, yPos);
            e.Graphics.DrawString("Advance: " + Advance.Text, subHeaderFont, Brushes.Black, leftMargin + 150, yPos);
            yPos += fontHeight * 2;
            // Print Separator Line
            e.Graphics.DrawString(new string('-', 80), printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;

            // Print Footer
            e.Graphics.DrawString("No warranty on mobile accessories, calculators,", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
            e.Graphics.DrawString("or any other electronic items.", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
            
            e.Graphics.DrawString("              Thank you for shopping.", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight * 2;

            // Print Separator Line
            e.Graphics.DrawString(new string('-', 80), printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
            e.Graphics.DrawString("Software Developed By AART Data Centre ", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
            e.Graphics.DrawString("Contact# 033-58445070", printFont, Brushes.Black, leftMargin, yPos);
            yPos += fontHeight;
        }







        // Method to get the next bill number
        private int getnextbillno()
        {
            int nextbillno = 1; // Default value in case of error
            try
            {
                string query = "SELECT MAX(Bill_No) AS MaxBillNo FROM Bill";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int billno = Convert.ToInt32(result);
                        nextbillno = billno + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return nextbillno;
        }
        private void DisplayNextBillNo()
        {
            int nextBillNo = getnextbillno();
            textBox18.Text = nextBillNo.ToString();
        }



        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void Balance_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

