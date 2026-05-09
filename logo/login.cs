using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;

namespace wasalha
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up f3 = new sign_up();
            f3.Show();
            this.Hide();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text.Trim();
            string password = txtpassword.Text.Trim();

            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            
            if (!rbclient.Checked && !rbsystemadmin.Checked)
            {
                MessageBox.Show("Please select a role (Client or Admin).");
                return;
            }

           
            string expectedtype = rbclient.Checked ? "customer" : "admin";

            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();

                string query = @"SELECT user_type, customer_id, employee_id 
                         FROM USERS 
                         WHERE username = @username 
                         AND password = @password 
                         AND user_type = @usertype";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@usertype", expectedtype);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string userType = reader["user_type"].ToString();

                    
                    if (userType == "admin")
                    {
                        int employeeId = Convert.ToInt32(reader["employee_id"]);
                        this.Hide();
                        adminHome adminForm = new adminHome(employeeId);
                        adminForm.Show();
                    }
                    
                    else if (userType == "customer")
                    {
                        int customerId = Convert.ToInt32(reader["customer_id"]);
                        this.Hide();
                        home customerForm = new home(customerId);
                        customerForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username, password, or user type.");
                }
            }
        }
    }
}
