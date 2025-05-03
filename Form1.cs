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
using System.Runtime.Remoting.Contexts;
namespace BOOKSWOOKS_PROJECT
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            this.Load += new EventHandler(Form1_Load); // Subscribe to the Load event
            FormClosing += Form1_LoadClosing;
        }
        private void Form1_LoadClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        public static class UserSession
        {
            public static string UserName { get; set; }
            public static string UserRole { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            SqlConnection con = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=BOOKSWOOKS;Integrated Security=True;");
 
            con.Open();
            // Query to check if the user exists and get the user role
            string query = "SELECT UserRole FROM UsersInfo WHERE UserName=@UserName AND Password=@Password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserName", user.Text);
            cmd.Parameters.AddWithValue("@Password", password.Text);

            object userRoleObj = cmd.ExecuteScalar();
            con.Close();

            if (userRoleObj != null)
            {
                string userRole = userRoleObj.ToString();
                // Store user information in UserSession
                UserSession.UserName = user.Text;
                UserSession.UserRole = userRole;
                if (userRole.Equals("Admin"))
                {
                    DASHBOARD DASHBOARDForm = new DASHBOARD();
                    DASHBOARDForm.Show();
                }
                else if (userRole.Equals("Assistant"))
                {
                    Invoice InvoiceForm = new Invoice();
                    InvoiceForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid user role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid User Name or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !checkBox1.Checked;
 
        }
    }
}
