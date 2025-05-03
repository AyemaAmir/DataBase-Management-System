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

namespace BOOKSWOOKS_PROJECT
{
    public partial class Manage_Users : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + System.IO.Path.GetFullPath("BOOKSWOOKS1.mdf") + "; Integrated Security = True; Connect Timeout = 30");

        bool sidebarExpand;
        public Manage_Users()
        {
            InitializeComponent();
            this.Load += new EventHandler(Manage_Users_Load); // Subscribe to the Load event
            FormClosing += Manage_Users_LoadClosing;
            cpass.UseSystemPasswordChar = true;
            npass.UseSystemPasswordChar = true;
            cpass1.UseSystemPasswordChar = true;
            npass1.UseSystemPasswordChar = true;

        }
        private void Manage_Users_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Manage_Users_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel23_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
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
            outcomes outcomesForm= new outcomes();
                        outcomesForm.Show();
                        this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Manage_Users manage_UsersForm= new Manage_Users();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string currentUserName = cname1.Text;
                string currentPassword = cpass1.Text;
                string newUserName = nname1.Text;
                string newPassword = npass1.Text;

                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(currentUserName) || string.IsNullOrEmpty(currentPassword))
                {
                    MessageBox.Show("Please enter the current username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }
                if (string.IsNullOrEmpty(newUserName) || string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Please enter the new username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                // SQL to check if the current username and password match and the user role is Admin
                string checkUserQuery = @"
        SELECT UserName, Password, UserRole
        FROM UsersInfo 
        WHERE UserName = @currentUserName AND Password = @currentPassword AND UserRole = 'Assistant'";

                using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, con))
                {
                    checkUserCmd.Parameters.AddWithValue("@currentUserName", currentUserName);
                    checkUserCmd.Parameters.AddWithValue("@currentPassword", currentPassword);

                    using (SqlDataReader reader = checkUserCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reader.Close(); // Close the reader before executing the update command

                            // SQL to update the username and password
                            string updateUserQuery = @"
                    UPDATE UsersInfo
                    SET UserName = @newUserName, Password = @newPassword
                    WHERE UserName = @currentUserName AND Password = @currentPassword AND UserRole = 'Assistant'";

                            using (SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, con))
                            {
                                updateUserCmd.Parameters.AddWithValue("@newUserName", newUserName);
                                updateUserCmd.Parameters.AddWithValue("@newPassword", newPassword);
                                updateUserCmd.Parameters.AddWithValue("@currentUserName", currentUserName);
                                updateUserCmd.Parameters.AddWithValue("@currentPassword", currentPassword);

                                int rowsAffected = updateUserCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Username and password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Current username and password do not match or user is not an Assistant.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

        }

       
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string currentUserName = cname.Text;
                string currentPassword = cpass.Text;
                string newUserName = nname.Text;
                string newPassword = npass.Text;

                // Validate user input (optional, but recommended)
                if (string.IsNullOrEmpty(currentUserName) || string.IsNullOrEmpty(currentPassword))
                {
                    MessageBox.Show("Please enter the current username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }
                if (string.IsNullOrEmpty(newUserName) || string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Please enter the new username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }

                // SQL to check if the current username and password match and the user role is Admin
                string checkUserQuery = @"
        SELECT UserName, Password, UserRole
        FROM UsersInfo 
        WHERE UserName = @currentUserName AND Password = @currentPassword AND UserRole = 'Admin'";

                using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, con))
                {
                    checkUserCmd.Parameters.AddWithValue("@currentUserName", currentUserName);
                    checkUserCmd.Parameters.AddWithValue("@currentPassword", currentPassword);

                    using (SqlDataReader reader = checkUserCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reader.Close(); // Close the reader before executing the update command

                            // SQL to update the username and password
                            string updateUserQuery = @"
                    UPDATE UsersInfo
                    SET UserName = @newUserName, Password = @newPassword
                    WHERE UserName = @currentUserName AND Password = @currentPassword AND UserRole = 'Admin'";

                            using (SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, con))
                            {
                                updateUserCmd.Parameters.AddWithValue("@newUserName", newUserName);
                                updateUserCmd.Parameters.AddWithValue("@newPassword", newPassword);
                                updateUserCmd.Parameters.AddWithValue("@currentUserName", currentUserName);
                                updateUserCmd.Parameters.AddWithValue("@currentPassword", currentPassword);

                                int rowsAffected = updateUserCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Username and password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Current username and password do not match or user is not an Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cpass.UseSystemPasswordChar = !checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            npass.UseSystemPasswordChar = !checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            cpass1.UseSystemPasswordChar = !checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            npass1.UseSystemPasswordChar = !checkBox4.Checked;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

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

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuButton_Click_1(object sender, EventArgs e)
        { //set timer interval to lowest to make it smooth bar
            sidebar1Timer.Start();

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Invoice InvoiceForm = new Invoice();
            InvoiceForm.Show();
            this.Hide();

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DASHBOARD DASHBOARDForm = new DASHBOARD();
            DASHBOARDForm.Show();
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
        {Manage_Users manage_UsersForm = new Manage_Users();
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            CreditCustomers EntryForm = new CreditCustomers();
            EntryForm.Show();
            this.Hide();

        }
    }
}
