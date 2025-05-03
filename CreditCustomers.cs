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
using System.Collections;


namespace BOOKSWOOKS_PROJECT
{
    public partial class CreditCustomers : Form
    {
        public static List<Tuple<int, string>> categoryList = new List<Tuple<int, string>>();
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
        public CreditCustomers()
        {
            InitializeComponent();
            this.Load += new EventHandler(CreditCustomers_Load); // Subscribe to the Load event
            FormClosing += CreditCustomers_LoadClosing;

        }
        private void CreditCustomers_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void CreditCustomers_Load(object sender, EventArgs e)
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

                // Loop through the rows and add the pairs to the list
                foreach (DataRow row in Category.Rows)
                {
                    int categoryID = Convert.ToInt32(row["Category_ID"]);
                    string categoryName = row["Category_Name"].ToString();
                    categoryList.Add(new Tuple<int, string>(categoryID, categoryName));
                }

                // Optionally set the DataSource of comboBox1 to display Category_Name
                comboBox1.DataSource = Category;
                comboBox1.DisplayMember = "Category_Name";

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
              CreditCustomers EntryForm=new CreditCustomers();
                        EntryForm.Show();
                        this.Hide();
        }

        private void sidebar_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Categories  categoriesForm= new Categories();
                            categoriesForm.Show();
                            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
                        outcomesForm.Show();
                    this.Hide();
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

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }
        // This function retrieves the product ID based on category (modify if using product name)
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
        private int? GetSupplierIDByName(string SupplierName)
        {
            string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Supplier_ID FROM Supplier WHERE Supplier_Name = @Supplier_Name";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Supplier_Name", SupplierName);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return (int?)Convert.ToInt32(result);
                    }
                    else
                    {
                        return null; // Supplier not found
                    }
                }
            }
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {
            try // Enclose data retrieval and database interaction in a try-catch block for error handling
            {
                con.Open();
                string productName = Product_Name.Text;
                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(productName))
                {
                    MessageBox.Show("Please enter a product name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                string categoryName = comboBox1.Text;  // Assuming comboBox1 is your ComboBox control

                if (string.IsNullOrEmpty(categoryName))
                {
                    MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // Find the Category_ID for the selected Category_Name
                int categoryID = -1; // Default value in case no match is found

                foreach (var tuple in categoryList)
                {
                    if (tuple.Item2 == categoryName)
                    {
                        categoryID = tuple.Item1;
                        break; // Exit the loop once a match is found
                    }
                }

                if (categoryID == -1)
                {
                    MessageBox.Show("Selected category is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if the category is not found
                }

                string query = @"
            SELECT p.*, pr.*
            FROM Product p
            INNER JOIN Purchase pr ON p.Product_ID = pr.Product_ID
            WHERE p.Product_Name = @ProductName";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Cost.Text = reader["Cost"].ToString();
                            Product_price.Text = reader["Price"].ToString();
                            Description.Text = reader["Description"].ToString();
                            Itms_per_pack.Text = reader["Items_per_unit"].ToString();
                            Brand.Text = reader["Brand"].ToString();
                           
                            dateTimePicker1.Value = Convert.ToDateTime(reader["Purchase_Date"]);
                            Qty_purchased.Text = reader["Qty_purchased"].ToString();
                            qty.Text = reader["Qty"].ToString();
                            
                            Total_amount_rolled.Text = reader["Total_Amount_Rolled"].ToString();
                           
                        }


                        else
                        {
                            MessageBox.Show("Product not found");
                        }
                    }
                }

            }
            catch (Exception ex) // Catch any exceptions during data processing or database interaction
            {
                MessageBox.Show("An error occurred while fetching the record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }

        }


        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string productName = Product_Name.Text;
                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(productName))
                {
                    MessageBox.Show("Please enter a product name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                string categoryName = comboBox1.Text; // Assuming comboBox1 is your ComboBox control
                                                      // Validate category selection (optional)
                if (string.IsNullOrEmpty(categoryName))
                {
                    MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Find the Category_ID for the selected Category_Name
                int categoryID = -1; // Default value in case no match is found

                foreach (var tuple in categoryList)
                {
                    if (tuple.Item2 == categoryName)
                    {
                        categoryID = tuple.Item1;
                        break; // Exit the loop once a match is found
                    }
                }

                if (categoryID == -1)
                {
                    MessageBox.Show("Selected category is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if the category is not found
                }
                float? cost = string.IsNullOrEmpty(Cost.Text) ? (float?)null : float.Parse(Cost.Text); // Allowing null values
                float? product_price = string.IsNullOrEmpty(Product_price.Text) ? (float?)null : float.Parse(Product_price.Text); // Allowing null value
                
                float? amount = string.IsNullOrEmpty(Total_amount_rolled.Text) ? (float?)null : float.Parse(Total_amount_rolled.Text); // Allowing null values
                DateTime? purchase_date = string.IsNullOrEmpty(dateTimePicker1.Text) ? (DateTime?)null : dateTimePicker1.Value;
                string description = string.IsNullOrEmpty(Description.Text) ? null : Description.Text; // Allowing null values
                string brand = string.IsNullOrEmpty(Brand.Text) ? null : Brand.Text;
          
                int? itms_per_pack = string.IsNullOrEmpty(Itms_per_pack.Text) ? (int?)null : int.Parse(Itms_per_pack.Text);
                float? Qtypurchased = string.IsNullOrEmpty(Qty_purchased.Text) ? (float?)null : float.Parse(Qty_purchased.Text);
                float? Qty = string.IsNullOrEmpty(qty.Text) ? (float?)null : float.Parse(qty.Text);
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Update Product table
                        string updateProductQuery = @"
                   UPDATE Product SET Cost = @Cost, Price = @Price,
                        Items_per_unit = @Items_per_unit, Description = @Description, Brand = @Brand,
                        Qty = @Qty
                    WHERE Product_Name = @Product_Name AND Category_ID = @Category_ID";

                        using (SqlCommand cmdProduct = new SqlCommand(updateProductQuery, con, transaction))
                        {
                            cmdProduct.Parameters.AddWithValue("@Cost", (object)cost ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Price", (object)product_price ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Items_per_unit", (object)itms_per_pack ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Brand", (object)brand ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Qty", (object)Qty ?? DBNull.Value);
                            cmdProduct.Parameters.AddWithValue("@Product_Name", productName);
                            cmdProduct.Parameters.AddWithValue("@Category_ID", categoryID);
                         
                            cmdProduct.ExecuteNonQuery();
                        }

                        // Update Purchase table
                        string updatePurchaseQuery = @"
                    UPDATE Purchase
                    SET 
                        Qty_purchased = @Qty_purchased,
                        Total_Amount_Rolled = @Total_Amount_Rolled,
                        Purchase_Date = @Purchase_Date
                    WHERE Product_ID = (SELECT Product_ID FROM Product WHERE Product_Name = @Product_Name)";

                        using (SqlCommand cmdPurchase = new SqlCommand(updatePurchaseQuery, con, transaction))
                        {
                           
                            cmdPurchase.Parameters.AddWithValue("@Qty_purchased", (object)Qtypurchased ?? DBNull.Value);
                            cmdPurchase.Parameters.AddWithValue("@Total_Amount_Rolled", (object)amount ?? DBNull.Value);
                            cmdPurchase.Parameters.AddWithValue("@Purchase_Date", (object)purchase_date ?? DBNull.Value);
                            cmdPurchase.Parameters.AddWithValue("@Product_Name", productName);
                            cmdPurchase.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Product and Purchase details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex) // Catch any exceptions during data processing or database interaction
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show("An error occurred while updating the record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data retrieval
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }

        }

        private void Product_Name_TextChanged(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                string SupplierName = string.IsNullOrEmpty(Supplier_Name.Text) ? null : Supplier_Name.Text;
                float? contact = string.IsNullOrEmpty(Contact.Text) ? (float?)null : float.Parse(Contact.Text); // Allowing null values
                float? account = string.IsNullOrEmpty(Account_No.Text) ? (float?)null : float.Parse(Account_No.Text);
                string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {

                        // Check if the supplier already exists and get its ID
                        int? supplierID = GetSupplierIDByName(SupplierName);

                        if (supplierID == null)
                        {
                            // Insert into Supplier table if the supplier does not exist
                            string query1 = "INSERT INTO Supplier (Supplier_Name, Contact, Account_No) " +
                                            "OUTPUT INSERTED.Supplier_ID VALUES (@Supplier_Name, @Contact, @Account_No)";
                            using (SqlCommand supplierCmd = new SqlCommand(query1, connection, transaction))
                            {
                                supplierCmd.Parameters.AddWithValue("@Supplier_Name", SupplierName);
                                supplierCmd.Parameters.AddWithValue("@Contact", contact);
                                supplierCmd.Parameters.AddWithValue("@Account_No", account);

                                supplierID = (int)supplierCmd.ExecuteScalar();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            MessageBox.Show("Supplier details added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear text boxes for new entry
                            Supplier_Name.Text = "";
                            Contact.Text = "";
                            Account_No.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Supplier already exists with ID: " + supplierID, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex) // Catch any exceptions during data processing or database interaction
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show("An error occurred while adding the supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data retrieval
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string supplierName = Supplier_Name.Text;

                // Validate user input
                if (string.IsNullOrEmpty(supplierName))
                {
                    MessageBox.Show("Please enter a Supplier name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                string query = "SELECT * FROM Supplier WHERE Supplier_Name = @Supplier_Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Correctly bind the parameter
                    cmd.Parameters.AddWithValue("@Supplier_Name", supplierName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Supplier_Name.Text = reader["Supplier_Name"].ToString();
                            Contact.Text = reader["Contact"].ToString();
                            Account_No.Text = reader["Account_No"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Supplier not found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching the Supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }

        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                // Assuming Product_ID is unique to each product, you may need to adjust this query accordingly
                string query = "UPDATE Supplier SET Supplier_Name = @Supplier_Name, Contact = @Contact, Account_No = @Account_No WHERE Supplier_Name = @Supplier_Name";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Ensure the correct data types are used
                    command.Parameters.AddWithValue("@Supplier_Name", string.IsNullOrEmpty(Supplier_Name.Text) ? DBNull.Value : (object)Supplier_Name.Text);
                    command.Parameters.AddWithValue("@Contact", string.IsNullOrEmpty(Contact.Text) ? DBNull.Value : (object)Contact.Text);
                    command.Parameters.AddWithValue("@Account_No", string.IsNullOrEmpty(Account_No.Text) ? DBNull.Value : (object)Account_No.Text);

                 

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear text boxes for new entry (optional)
                        Supplier_Name.Text = "";
                        Contact.Text = "";
                        Account_No.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No Supplier found to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the Supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                string SupplierName = Supplier_Name.Text;
                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(SupplierName))
                {
                    MessageBox.Show("Please enter a Supplier name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }
               
                string query = "DELETE FROM Supplier WHERE Supplier_Name = @Supplier_Name";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Ensure the correct data type is used
                    command.Parameters.AddWithValue("@Supplier_Name", SupplierName);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear text boxes for new entry (optional)
                        Supplier_Name.Text = "";
                        Contact.Text = "";
                        Account_No.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No Supplier found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data processing or database interaction
            {
                MessageBox.Show("An error occurred while deleting the Supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }
        }

        private void Total_amount_rolled_TextChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve and validate values from textboxes
                if (int.TryParse(Qty_purchased.Text, out int qtyPurchased) && int.TryParse(qty.Text, out int currentQty))
                {
                    // Calculate the sum
                    int newQty = qtyPurchased + currentQty;

                    // Display the sum in the newqty textbox
                    newqty.Text = newQty.ToString();

                    // Get Product ID
                    int productID = GetProductIDByProductName(Product_Name.Text);
                    if (productID == -1)
                    {
                        MessageBox.Show("Product not found.");
                        return;
                    }

                    // Get Supplier ID (assuming you have a method or a control to get supplier name)
                    int? supplierID = GetSupplierIDByName(Supplier_Name.Text); // Replace Supplier_Name.Text with the actual supplier name input control
                    if (supplierID == null)
                    {
                        MessageBox.Show("Supplier not found.");
                        return;
                    }

                    // Get Category ID
                    int categoryID = -1; // Default value in case no match is found
                    foreach (var tuple in categoryList)
                    {
                        if (tuple.Item2 == comboBox1.Text)
                        {
                            categoryID = tuple.Item1;
                            break; // Exit the loop once a match is found
                        }
                    }

                    if (categoryID == -1)
                    {
                        MessageBox.Show("Selected category is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Exit the method if the category is not found
                    }
                    // Validate and parse Total_Amount_Rolled
                    if (!float.TryParse(Total_amount_rolled.Text, out float totalAmountRolled))
                    {
                        MessageBox.Show("Please enter a valid numeric value for Total Amount Rolled.");
                        return;
                    }

                    // Update the qty column in the product table
                    string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string query = "UPDATE product SET qty = @Qty WHERE Product_Name = @Product_Name";

                                using (SqlCommand updateCommand = new SqlCommand(query, connection, transaction))
                                {
                                    updateCommand.Parameters.AddWithValue("@Qty", newQty);
                                    updateCommand.Parameters.AddWithValue("@Product_Name", Product_Name.Text);
                                    updateCommand.ExecuteNonQuery();
                                }
                                // Insert into purchase table
                                string insertPurchaseQuery = @"
                            INSERT INTO Purchase (Qty_purchased, Product_ID, Supplier_ID, Purchase_Date, Category_ID,Total_Amount_Rolled)
                            VALUES (@Qty_purchased, @Product_ID, @Supplier_ID, @Purchase_Date, @Category_ID,@Total_Amount_Rolled)";

                                using (SqlCommand insertCommand = new SqlCommand(insertPurchaseQuery, connection, transaction))
                                {
                                    insertCommand.Parameters.AddWithValue("@Qty_purchased", qtyPurchased);
                                    insertCommand.Parameters.AddWithValue("@Total_Amount_Rolled", totalAmountRolled);
                                    insertCommand.Parameters.AddWithValue("@Product_ID", productID);
                                    insertCommand.Parameters.AddWithValue("@Supplier_ID", supplierID);
                                    insertCommand.Parameters.AddWithValue("@Purchase_Date", dateTimePicker1.Value);
                                    insertCommand.Parameters.AddWithValue("@Category_ID", categoryID);
                                    insertCommand.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();

                                MessageBox.Show("Quantity updated successfully.");

                                // Clear the textboxes
                                Qty_purchased.Text = "";
                                newqty.Text = "";
                                qty.Text = "";
                                Total_amount_rolled.Text = "";
                                Product_Name.Text = "";
                                Brand.Text = "";
                                Description.Text = "";
                                Product_price.Text = "";
                                Cost.Text = "";
                                Itms_per_pack.Text = "";
                               
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if an error occurs
                                transaction.Rollback();
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid numeric values for quantities.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try // Enclose data retrieval and database interaction in a try-catch block for error handling
            {
                string productName = Product_Name.Text;


                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(productName))
                {
                    MessageBox.Show("Please enter a product name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                string categoryName = comboBox1.Text; // Assuming comboBox1 is your ComboBox control

                if (string.IsNullOrEmpty(categoryName))
                {
                    MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if no category is selected
                }

                // Find the Category_ID for the selected Category_Name
                int categoryID = -1; // Default value in case no match is found

                foreach (var tuple in categoryList)
                {
                    if (tuple.Item2 == categoryName)
                    {
                        categoryID = tuple.Item1;
                        break; // Exit the loop once a match is found
                    }
                }

                if (categoryID == -1)
                {
                    MessageBox.Show("Selected category is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if the category is not found
                }

                float? cost = string.IsNullOrEmpty(Cost.Text) ? (float?)null : float.Parse(Cost.Text); // Allowing null values
                float? product_price = string.IsNullOrEmpty(Product_price.Text) ? (float?)null : float.Parse(Product_price.Text); // Allowing null value
                
                float? amount = string.IsNullOrEmpty(Total_amount_rolled.Text) ? (float?)null : float.Parse(Total_amount_rolled.Text); // Allowing null values
                DateTime? purchase_date = string.IsNullOrEmpty(dateTimePicker1.Text) ? (DateTime?)null : dateTimePicker1.Value;
                string description = string.IsNullOrEmpty(Description.Text) ? null : Description.Text; // Allowing null values
                string brand = string.IsNullOrEmpty(Brand.Text) ? null : Brand.Text;

                int? itms_per_pack = string.IsNullOrEmpty(Itms_per_pack.Text) ? (int?)null : int.Parse(Itms_per_pack.Text);
                string SupplierName = string.IsNullOrEmpty(Supplier_Name.Text) ? null : Supplier_Name.Text;
                float? Qtypurchased = string.IsNullOrEmpty(Qty_purchased.Text) ? (float?)null : float.Parse(Qty_purchased.Text);

                string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        int? supplierID = GetSupplierIDByName(SupplierName); ;
                        int productID = GetProductIDByProductName(productName);
                        // Create a SqlCommand object for the stored procedure
                        SqlCommand cmd = new SqlCommand("Insertproductpurchaserecord777333", connection, transaction);
                        cmd.CommandType = CommandType.StoredProcedure; // Specify it's a stored procedure
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@Cost", (object)cost ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Price", (object)product_price ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@itms_per_pack",(object)itms_per_pack ?? DBNull.Value);    
                      
                        cmd.Parameters.AddWithValue("@Qty_purchased", (object)Qtypurchased ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Brand", (object)brand ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        cmd.Parameters.AddWithValue("@TotalAmountRolled",(object)amount ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PurchaseDate", (object)purchase_date ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Product_ID", productID);
                        cmd.ExecuteNonQuery();

                        
                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Product and Purchase details added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear text boxes for new entry (optional)
                        Product_Name.Text = "";
                        Cost.Text = "";
                        Product_price.Text = "";
                        qty.Text = "";
                        Qty_purchased.Text = "";
                       
                        Itms_per_pack.Text = "";
                        newqty.Text = "";
                        Total_amount_rolled.Text = "";
                       
                        Brand.Text = "";
                        Description.Text = "";
                    }
                    catch (Exception ex) // Catch any exceptions during data processing or database interaction
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show("An error occurred while adding the product and purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data retrieval
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
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

        private void Itms_per_pack_TextChanged(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DASHBOARD dASHBOARDForm= new DASHBOARD();
            dASHBOARDForm.Show();
            this.Hide();
        }

        private void buttondashboard_Click_1(object sender, EventArgs e)
        {
            DASHBOARD dASHBOARDForm = new DASHBOARD();
            dASHBOARDForm.Show();
            this.Hide();

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Invoice invoiceForm = new Invoice();
                invoiceForm.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
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

        private void button3_Click_1(object sender, EventArgs e)
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
            this.Hide();

        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            outcomes outcomesForm = new outcomes();
            outcomesForm.Show();
            this.Hide();
        }

        private void button6_Click_1(object sender, EventArgs e)
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

        private void button7_Click_1(object sender, EventArgs e)
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

        private void button8_Click_1(object sender, EventArgs e)
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
    }
}
