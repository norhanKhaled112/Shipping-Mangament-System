using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using wasalha;

namespace wasalha
{
    public partial class profile : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _customerId;

        public profile(int customerId)
        {
            _customerId = customerId;
            this.Size = new Size(1030, 760);
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Text = "Wasalha System";
            this.StartPosition = FormStartPosition.CenterScreen;
            BuildUI();
        }

        private void BuildUI()
        {
            // SIDEBAR
            Panel sidebar = new Panel
            {
                Width = 200,
                Dock = DockStyle.Left,
                BackColor = Color.White
            };
            this.Controls.Add(sidebar);

            PictureBox logo = new PictureBox
            {
                Size = new Size(160, 90),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            try { logo.Image = Image.FromFile(@"C:\system-analysis\logo\logo\logo.png"); } catch { }
            sidebar.Controls.Add(logo);

            string[] items = { "Home", "Shipment", "Tracking", "Profile", "Logout","Help" };
            for (int i = 0; i < items.Length; i++)
            {
                Button btn = new Button
                {
                    Text = "      " + items[i],
                    Size = new Size(200, 50),
                    Location = new Point(0, 150 + (i * 55)),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI Semibold", 10),
                    ForeColor = darkBlue,
                    BackColor = Color.White
                };
                btn.FlatAppearance.BorderSize = 0;

                if (items[i] == "Profile")
                {
                    btn.BackColor = mainBlue;
                    btn.ForeColor = Color.White;
                }

                string itemName = items[i];
                if (itemName == "Logout")
                    btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                else if (itemName == "Home")
                    btn.Click += (s, e) => { new home(_customerId).Show(); this.Hide(); };
                else if (itemName == "Shipment")
                    btn.Click += (s, e) => { new clientship(_customerId).Show(); this.Hide(); };
                else if (itemName == "Tracking")
                    btn.Click += (s, e) => { new trackingclient(_customerId).Show(); this.Hide(); };
                else if (itemName == "Help")
                    btn.Click += (s, e) => { new help(_customerId).Show(); this.Hide(); };

                sidebar.Controls.Add(btn);
            }

            // NAVBAR
            Panel navbar = new Panel
            {
                Height = 80,
                Width = this.Width - 200,
                Location = new Point(200, 0),
                BackColor = mainBlue
            };
            this.Controls.Add(navbar);

            Label title = new Label
            {
                Text = "My Profile",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 16),
                AutoSize = false,
                Size = new Size(navbar.Width, navbar.Height),
                TextAlign = ContentAlignment.MiddleCenter
            };
            navbar.Controls.Add(title);

            // MAIN AREA
            Panel mainArea = new Panel
            {
                Location = new Point(200, 80),
                Size = new Size(this.Width - 200, this.Height - 80),
                BackColor = bgGray
            };
            this.Controls.Add(mainArea);

            // جيب البيانات من الداتا بيز
            string username = "", phone = "", address = "",  createdAt = "";
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT  c.phone, c.address, u.username, u.created_at
                                 FROM CUSTOMER c
                                 JOIN USERS u ON c.customer_id = u.customer_id
                                 WHERE c.customer_id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", _customerId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    username = reader["username"].ToString();
                    phone = reader["phone"].ToString();
                    address = reader["address"].ToString();
                    createdAt = reader["created_at"] == DBNull.Value ? "N/A":
                    createdAt = Convert.ToDateTime(reader["created_at"]).ToString("dd MMM yyyy");
                }
            }

            // GROUPBOX
            GroupBox grpInfo = new GroupBox
            {
                Text = "Personal Information",
                Location = new Point(80, 40),
                Size = new Size(720, 280),
                Font = new Font("Segoe UI Semibold", 10),
                ForeColor = darkBlue,
                BackColor = Color.White
            };
            mainArea.Controls.Add(grpInfo);

            string[] labels = { "Username:", "Phone:", "Address:", "Account Created:" };
            string[] values = { username, phone, address, createdAt };

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = labels[i],
                    Location = new Point(30, 50 + (i * 50)),
                    Size = new Size(150, 25),
                    Font = new Font("Segoe UI Semibold", 10),
                    ForeColor = darkBlue
                };
                Label val = new Label
                {
                    Text = values[i],
                    Location = new Point(200, 50 + (i * 50)),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Black
                };
                grpInfo.Controls.Add(lbl);
                grpInfo.Controls.Add(val);
            }

            // STATUS BAR
            Panel statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(224, 224, 224)
            };
           
           
        }
    }
}
