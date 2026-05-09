using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wasalha
{
    public partial class sign_up : Form
    {
        public sign_up()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void rbsystemadmin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbsystemadmin.Checked)
            {
                clientpanel.Visible = false;
                adminpanel.Visible = true;
            }

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void rbclient_CheckedChanged(object sender, EventArgs e)
        {
            if (rbclient.Checked)
            {
                clientpanel.Visible = true;
                adminpanel.Visible = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void btnsigup_Click(object sender, EventArgs e)
        {
            if (rbclient.Checked)
            {
                RegisterClient();
            }
            else if (rbsystemadmin.Checked)
            {
                RegisterAdmin();
            }
            else
            {
                MessageBox.Show("Please select a role (Client or Admin).");
            }
        }
        private void RegisterClient()
        {
            string phone = txtclientphone.Text.Trim();
            string username = txtclientusername.Text.Trim();
            string address = txtclientaddress.Text.Trim();
            string password = txtclientpassword.Text.Trim();

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(address) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter all required fields.");
                return;
            }

            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertCustomer = @"INSERT INTO CUSTOMER (name, phone, address) 
                                            VALUES (@name, @phone, @address);
                                            SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdCustomer = new SqlCommand(insertCustomer, conn, transaction);
                    cmdCustomer.Parameters.AddWithValue("@name", username);
                    cmdCustomer.Parameters.AddWithValue("@phone", phone);
                    cmdCustomer.Parameters.AddWithValue("@address", address);

                    int customerId = Convert.ToInt32(cmdCustomer.ExecuteScalar());

                    string insertUser = @"INSERT INTO USERS (username, password, user_type, customer_id) 
                                        VALUES (@username, @password, 'customer', @customerId)";

                    SqlCommand cmdUser = new SqlCommand(insertUser, conn, transaction);
                    cmdUser.Parameters.AddWithValue("@username", username);
                    cmdUser.Parameters.AddWithValue("@password", password);
                    cmdUser.Parameters.AddWithValue("@customerId", customerId);

                    cmdUser.ExecuteNonQuery();
                    transaction.Commit();

                    MessageBox.Show("Sign up successfully!");
                    this.Hide();
                    login loginForm = new login();
                    loginForm.Show();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error " + ex.Message);
                }
            }
        }

        private void RegisterAdmin()
        {
            string name = txtadminusername.Text.Trim();
            string phone = txtadminphone.Text.Trim();
            string password = txtadminpassword.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter all required fields.");
                return;
            }

            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertEmployee = @"INSERT INTO EMPLOYEE (name, phone) 
                                            VALUES (@name, @phone);
                                            SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdEmployee = new SqlCommand(insertEmployee, conn, transaction);
                    cmdEmployee.Parameters.AddWithValue("@name", name);
                    cmdEmployee.Parameters.AddWithValue("@phone", phone);

                    int employeeId = Convert.ToInt32(cmdEmployee.ExecuteScalar());

                    string insertUser = @"INSERT INTO USERS (username, password, user_type, employee_id) 
                                        VALUES (@username, @password, 'admin', @employeeId)";

                    SqlCommand cmdUser = new SqlCommand(insertUser, conn, transaction);
                    cmdUser.Parameters.AddWithValue("@username", name);
                    cmdUser.Parameters.AddWithValue("@password", password);
                    cmdUser.Parameters.AddWithValue("@employeeId", employeeId);

                    cmdUser.ExecuteNonQuery();
                    transaction.Commit();

                    MessageBox.Show("Admin registered successfully!");
                    this.Hide();
                    login loginForm = new login();
                    loginForm.Show();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error " + ex.Message);
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
