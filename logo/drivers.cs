using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class drivers : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;
        private DataGridView dgv;

        public drivers(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            this.Size = new Size(1030, 760);
            this.BackColor = bgGray;
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

            string[] items = { "Home", "Shipment", "Tracking", "Drivers", "Reports", "Feedback", "Logout" };
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
                    ForeColor = darkBlue
                };
                btn.FlatAppearance.BorderSize = 0;

                string itemName = items[i];
                if (itemName == "Logout")
                    btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                else if (itemName == "Home")
                    btn.Click += (s, e) => { new adminHome(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Shipment")
                    btn.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Tracking")
                    btn.Click += (s, e) => { new trackingadmin(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Feedback")
                    btn.Click += (s, e) => { new feedback(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Drivers")
                    btn.BackColor = Color.FromArgb(220, 235, 245);

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
                Text = "Drivers Management",
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

            // CARDS
            Panel cardAll = CreateCard("Total Drivers", GetCount("SELECT COUNT(*) FROM DRIVER").ToString(), 20, mainArea);
            Panel cardAvail = CreateCard("Available", GetCount("SELECT COUNT(*) FROM DRIVER WHERE status='available'").ToString(), 195, mainArea);
            Panel cardBusy = CreateCard("Busy", GetCount("SELECT COUNT(*) FROM DRIVER WHERE status='busy'").ToString(), 370, mainArea);

            // FILTER BUTTONS
            Button btnAll = new Button
            {
                Text = "All",
                Size = new Size(100, 35),
                Location = new Point(20, 110),
                BackColor = mainBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnAll.FlatAppearance.BorderSize = 0;

            Button btnAvailable = new Button
            {
                Text = "Available",
                Size = new Size(100, 35),
                Location = new Point(130, 110),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnAvailable.FlatAppearance.BorderSize = 0;

            Button btnBusy = new Button
            {
                Text = "Busy",
                Size = new Size(100, 35),
                Location = new Point(240, 110),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnBusy.FlatAppearance.BorderSize = 0;

            mainArea.Controls.Add(btnAll);
            mainArea.Controls.Add(btnAvailable);
            mainArea.Controls.Add(btnBusy);

            // TABLE
            dgv = new DataGridView
            {
                Location = new Point(20, 160),
                Size = new Size(770, 380),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dgv.Columns.Add("driver_id", "ID");
            dgv.Columns.Add("name", "Name");
            dgv.Columns.Add("phone", "Phone");
            dgv.Columns.Add("status", "Status");

            mainArea.Controls.Add(dgv);
            LoadDrivers("all");

            // FILTER EVENTS
            btnAll.Click += (s, e) => { dgv.Rows.Clear(); LoadDrivers("all"); };
            btnAvailable.Click += (s, e) => { dgv.Rows.Clear(); LoadDrivers("available"); };
            btnBusy.Click += (s, e) => { dgv.Rows.Clear(); LoadDrivers("busy"); };

            // UPDATE STATUS PANEL
            Panel updatePanel = new Panel
            {
                Location = new Point(20, 555),
                Size = new Size(770, 60),
                BackColor = Color.White
            };
            mainArea.Controls.Add(updatePanel);

            Label lblStatus = new Label
            {
                Text = "Change Status:",
                Location = new Point(15, 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = darkBlue
            };
            updatePanel.Controls.Add(lblStatus);

            ComboBox cmbStatus = new ComboBox
            {
                Location = new Point(140, 13),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbStatus.Items.AddRange(new string[] { "available", "busy" });
            updatePanel.Controls.Add(cmbStatus);

            Button btnUpdate = new Button
            {
                Text = "Update",
                Size = new Size(120, 35),
                Location = new Point(310, 10),
                BackColor = mainBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            updatePanel.Controls.Add(btnUpdate);

            btnUpdate.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Choose a driver");
                    return;
                }
                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Choose a status!");
                    return;
                }

                int driverId = Convert.ToInt32(dgv.SelectedRows[0].Cells["driver_id"].Value);
                string newStatus = cmbStatus.SelectedItem.ToString();

                using (SqlConnection conn = DATABASE.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE DRIVER SET status=@status WHERE driver_id=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", driverId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Updated successfully!");
                dgv.Rows.Clear();
                LoadDrivers("all");
            };
        }

        private Panel CreateCard(string label, string value, int x, Panel parent)
        {
            Panel card = new Panel
            {
                Size = new Size(160, 80),
                Location = new Point(x, 20),
                BackColor = Color.White
            };

            card.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = value,
                Location = new Point(10, 35),
                Font = new Font("Segoe UI Bold", 14),
                ForeColor = mainBlue,
                AutoSize = true
            });

            parent.Controls.Add(card);
            return card;
        }

        private int GetCount(string query)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                return (int)cmd.ExecuteScalar();
            }
        }

        private void LoadDrivers(string filter)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = filter == "all"
                    ? "SELECT driver_id, name, phone, status FROM DRIVER ORDER BY name"
                    : "SELECT driver_id, name, phone, status FROM DRIVER WHERE status=@status ORDER BY name";

                SqlCommand cmd = new SqlCommand(query, conn);
                if (filter != "all")
                    cmd.Parameters.AddWithValue("@status", filter);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgv.Rows.Add(
                        reader["driver_id"].ToString(),
                        reader["name"].ToString(),
                        reader["phone"].ToString(),
                        reader["status"].ToString()
                    );
                }
            }
        }

        private void adminDrivers_Load(object sender, EventArgs e) { }
    }
}
