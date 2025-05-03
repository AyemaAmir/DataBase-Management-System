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

namespace BOOKSWOOKS_PROJECT
{
    public partial class Product : Form
    {
        // Declare the global variable as static
        public static List<Tuple<int, string>> categoryList = new List<Tuple<int, string>>();
        bool sidebarExpand;
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
        public Product()
        {
            InitializeComponent();
            this.Load += new EventHandler(Product_Load); // Subscribe to the Load event
            FormClosing += Product_FormClosing;

        }
        private void Product_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
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
        private void pictureBox1_Click1(object sender, EventArgs e)
        {
           
        }

        private void sidebar1Timer_Tick(object sender, EventArgs e)
        {

        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
           
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
                float? price = string.IsNullOrEmpty(Price.Text) ? (float?)null : float.Parse(Price.Text); // Allowing null values
                int? itemsPerUnit = string.IsNullOrEmpty(ItemsPerUnit.Text) ? (int?)null : int.Parse(ItemsPerUnit.Text); // Allowing null values
                string description = string.IsNullOrEmpty(Description.Text) ? null : Description.Text; // Allowing null values
                string brand = string.IsNullOrEmpty(Brand.Text) ? null : Brand.Text; // Allowing null values
                DateTime? dateOfEntry = string.IsNullOrEmpty(dateTimePicker1.Text) ? (DateTime?)null : dateTimePicker1.Value; // Allowing null values
                float? unitCost = string.IsNullOrEmpty(UnitCost.Text) ? (float?)null : float.Parse(UnitCost.Text); // Allowing null values
                float? unitPrice = string.IsNullOrEmpty(UnitPrice.Text) ? (float?)null : float.Parse(UnitPrice.Text); // Allowing null values


                string connectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");
                string query = "INSERT INTO Product (Product_Name, Category_ID, Cost, Unit_Price, Price, Items_per_unit, Description, Brand, Date_of_Entry, Unit_Cost) " +
                               "VALUES (@Product_Name, @Category_ID, @Cost, @Unit_Price, @Price, @Items_per_unit, @Description, @Brand, @Date_of_Entry, @Unit_Cost)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Product_Name", productName);
                        command.Parameters.AddWithValue("@Category_ID", categoryID);
                        command.Parameters.AddWithValue("@Cost", (object)cost ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Price", (object)price ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Items_per_unit", (object)itemsPerUnit ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Brand", (object)brand ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Date_of_Entry", (object)dateOfEntry ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Unit_Cost", (object)unitCost ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Unit_Price", (object)unitPrice ?? DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                    // Clear text boxes for new entry (optional)
                    Product_Name.Text = "";
                    Cost.Text = "";
                    UnitPrice.Text = "";
                    ItemsPerUnit.Text = "";
                    Description.Text = "";
                    Brand.Text = "";
                    Price.Text = "";
                    UnitCost.Text = "";

                    // Display success message (optional)
                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) // Catch any exceptions during data processing or database interaction
            {
                MessageBox.Show("An error occurred while adding the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button17_Click(object sender, EventArgs e)
        {

           
        }

        private void button17_Click_1(object sender, EventArgs e)
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

                string query = "SELECT * FROM Product WHERE Product_Name = @ProductName AND Category_ID = @CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Cost.Text = reader["Cost"].ToString();
                            Price.Text = reader["Price"].ToString();
                            Description.Text = reader["Description"].ToString();
                            ItemsPerUnit.Text = reader["Items_per_unit"].ToString();
                            Brand.Text = reader["Brand"].ToString();
                            UnitPrice.Text = reader["Unit_Price"].ToString();
                            UnitCost.Text = reader["Unit_Cost"].ToString();
                            dateTimePicker1.Value = Convert.ToDateTime(reader["Date_of_Entry"]);
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
                MessageBox.Show("An error occurred while fetching the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }
        }

        private void button5_Click(object sender, EventArgs e)
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

                // Assuming Product_ID is unique to each product, you may need to adjust this query accordingly
                string query = "UPDATE Product SET Cost = @Cost, Unit_Price = @Unit_Price, Price = @Price, " +
                               "Items_per_unit = @Items_per_unit, Description = @Description, Brand = @Brand, " +
                               "Date_of_Entry = @Date_of_Entry, Unit_Cost = @Unit_Cost WHERE Product_Name = @ProductName AND Category_ID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Cost", string.IsNullOrEmpty(Cost.Text) ? DBNull.Value : (object)float.Parse(Cost.Text));
                    command.Parameters.AddWithValue("@Unit_Price", string.IsNullOrEmpty(UnitPrice.Text) ? DBNull.Value : (object)float.Parse(UnitPrice.Text));
                    command.Parameters.AddWithValue("@Price", string.IsNullOrEmpty(Price.Text) ? DBNull.Value : (object)float.Parse(Price.Text));
                    command.Parameters.AddWithValue("@Items_per_unit", string.IsNullOrEmpty(ItemsPerUnit.Text) ? DBNull.Value : (object)int.Parse(ItemsPerUnit.Text));
                    command.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(Description.Text) ? DBNull.Value : (object)Description.Text);
                    command.Parameters.AddWithValue("@Brand", string.IsNullOrEmpty(Brand.Text) ? DBNull.Value : (object)Brand.Text);
                    command.Parameters.AddWithValue("@Date_of_Entry", dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@Unit_Cost", string.IsNullOrEmpty(UnitCost.Text) ? DBNull.Value : (object)float.Parse(UnitCost.Text));
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@CategoryID", categoryID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear text boxes for new entry (optional)
                        Product_Name.Text = "";
                        Cost.Text = "";
                        UnitPrice.Text = "";
                        ItemsPerUnit.Text = "";
                        Description.Text = "";
                        Brand.Text = "";
                        Price.Text = "";
                        UnitCost.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No product found to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data processing or database interaction
            {
                MessageBox.Show("An error occurred while updating the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string productName = Product_Name.Text;
                // Validate user input
                if (string.IsNullOrEmpty(productName))
                {
                    MessageBox.Show("Please enter a product name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string categoryName = comboBox1.Text; // Assuming comboBox1 is your ComboBox control
                                                      // Validate category selection
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

                string query = "DELETE FROM Product WHERE Product_Name = @ProductName AND Category_ID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@CategoryID", categoryID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear text boxes for new entry (optional)
                        Product_Name.Text = "";
                        Cost.Text = "";
                        UnitPrice.Text = "";
                        ItemsPerUnit.Text = "";
                        Description.Text = "";
                        Brand.Text = "";
                        Price.Text = "";
                        UnitCost.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No product found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) // Catch any exceptions during data processing or database interaction
            {
                MessageBox.Show("An error occurred while deleting the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close(); // Ensure the connection is closed regardless of success or failure
            }
        }

        private void menuButton_Click_1(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smooth bar
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
            Product ProductForm = new Product();
            ProductForm.Show();
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
            outcomesForm.Show();
            this.Hide();

        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            Manage_Users manage_usersForm = new Manage_Users();
            manage_usersForm.Show();
            this.Hide();
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            this.Close();

            // Show the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void sidebar1Timer_Tick_1(object sender, EventArgs e)
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

        private void Cost_TextChanged(object sender, EventArgs e)
        {

        }

        private void Product_Name_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
