using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class trackingadmin: Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;

        public trackingadmin(int employeeId)
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

            string[] items = { "Home", "Shipment", "Tracking","Reports", "Feedback", "Logout" };
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
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(91, 158, 201); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
                string itemName = items[i];
                if (itemName == "Logout")
                    btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                else if (itemName == "Home")
                    btn.Click += (s, e) => { new adminHome(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Shipment")
                    btn.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Feedback")
                    btn.Click += (s, e) => { new feedback(_employeeId).Show(); this.Hide(); };
                else if( itemName == "Drivers")
                    btn.Click += (s, e) => { new drivers(_employeeId).Show(); this.Hide(); };
                else if( itemName == "Reports")
                    btn.Click += (s, e) => { new reports(_employeeId).Show(); this.Hide(); };
                

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
                Text = "Shipment Tracking",
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

            // TABLE
            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(770, 400),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Name = "dgvTracking"
            };

            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dgv.Columns.Add("shipment_id", "ID");
            dgv.Columns.Add("customer", "Customer");
            dgv.Columns.Add("phone", "Phone");
            dgv.Columns.Add("from", "From");
            dgv.Columns.Add("to", "To");
            dgv.Columns.Add("status", "Status");
            dgv.Columns.Add("date", "Date");
            dgv.ReadOnly = true;

            mainArea.Controls.Add(dgv);
            LoadShipments(dgv);

            // UPDATE STATUS PANEL
            Panel updatePanel = new Panel
            {
                Location = new Point(20, 440),
                Size = new Size(770, 80),
                BackColor = Color.White
            };
            mainArea.Controls.Add(updatePanel);

            Label lblSelected = new Label
            {
                Text = "Selected Shipment ID:",
                Location = new Point(15, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = darkBlue
            };
            updatePanel.Controls.Add(lblSelected);

            Label lblId = new Label
            {
                Text = "-",
                Location = new Point(180, 25),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 10),
                ForeColor = mainBlue,
                Name = "lblSelectedId"
            };
            updatePanel.Controls.Add(lblId);

            Label lblStatus = new Label
            {
                Text = "New Status:",
                Location = new Point(280, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = darkBlue
            };
            updatePanel.Controls.Add(lblStatus);

            ComboBox cmbStatus = new ComboBox
            {
                Location = new Point(380, 20),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbStatus.Items.AddRange(new string[] { "pending", "in transit", "delivered", "cancelled" });
            updatePanel.Controls.Add(cmbStatus);

            Button btnUpdate = new Button
            {
                Text = "Update Status",
                Size = new Size(140, 35),
                Location = new Point(545, 18),
                BackColor = mainBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            updatePanel.Controls.Add(btnUpdate);

            // 
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                    lblId.Text = dgv.SelectedRows[0].Cells["shipment_id"].Value.ToString();
            };

            // update btn
            btnUpdate.Click += (s, e) =>
            {
                if (lblId.Text == "-")
                {
                    MessageBox.Show("Choose a shipment first!");
                    return;
                }
                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Choose a new status!");
                    return;
                }

                int shipmentId = Convert.ToInt32(lblId.Text);
                string newStatus = cmbStatus.SelectedItem.ToString();

                using (SqlConnection conn = DATABASE.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE SHIPMENT SET status=@status WHERE shipment_id=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", shipmentId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Status updated successfully!");
                dgv.Rows.Clear();
                LoadShipments(dgv);
            };
        }

        private void LoadShipments(DataGridView dgv)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    s.shipment_id,
                                    c.name,
                                    c.phone,
                                    s.pickup_address,
                                    s.delivery_address,
                                    s.status,
                                    s.created_at
                                 FROM SHIPMENT s
                                 JOIN CUSTOMER c ON s.customer_id = c.customer_id
                                 ORDER BY s.created_at DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgv.Rows.Add(
                        reader["shipment_id"].ToString(),
                        reader["name"].ToString(),
                        reader["phone"].ToString(),
                        reader["pickup_address"].ToString(),
                        reader["delivery_address"].ToString(),
                        reader["status"].ToString(),
                        Convert.ToDateTime(reader["created_at"]).ToString("d MMM yyyy")
                    );
                }
            }
        }

        private void adminTracking_Load(object sender, EventArgs e) { }

        private void trackingadmin_Load(object sender, EventArgs e)
        {

        }
    }
}