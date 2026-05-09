using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class trackingclient : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _customerId;

        public trackingclient(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
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

            string[] items = { "Home", "Shipment", "Tracking", "Profile", "Logout", "Help" };
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

                if (items[i] == "Tracking")
                {
                    btn.BackColor = mainBlue;
                    btn.ForeColor = Color.White;
                }

                string itemName = items[i];
                if (itemName == "Logout")
                    btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                else if (itemName == "Home")
                    btn.Click += (s, e) => { new home(_customerId).Show(); this.Hide(); };
                else if (itemName == "Help")
                    btn.Click += (s, e) => { new    help(_customerId).Show(); this.Hide(); };
                else if (itemName == "Profile")
                    btn.Click += (s, e) => { new profile(_customerId).Show(); this.Hide(); };
                else if (itemName == "Shipment")
                    btn.Click += (s, e) => { new clientship(_customerId).Show(); this.Hide(); };

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
                Text = "My Shipments Tracking",
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
                Size = new Size(770, 500),
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);

            dgv.Columns.Add("shipment_id", "Shipment ID");
            dgv.Columns.Add("from", "From");
            dgv.Columns.Add("to", "To");
            dgv.Columns.Add("status", "Status");
         
            dgv.Columns.Add("date", "Date");
            dgv.ReadOnly = true;

            mainArea.Controls.Add(dgv);
            LoadShipments(dgv);

            // STATUS BAR
            Panel statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.White
            };
            
        }

        private void LoadShipments(DataGridView dgv)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    s.shipment_id,
                                    s.pickup_address,
                                    s.delivery_address,
                                    s.status,
                        
                                    s.created_at
                                 FROM SHIPMENT s
                                 
                                 WHERE s.customer_id = @id
                                 ORDER BY s.created_at DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", _customerId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string status = reader["status"].ToString();
                    int rowIndex = dgv.Rows.Add(
                        reader["shipment_id"].ToString(),
                        reader["pickup_address"].ToString(),
                        reader["delivery_address"].ToString(),
                        status,
              
                        Convert.ToDateTime(reader["created_at"]).ToString("d MMM yyyy")
                    );

                    // لون الحالة
                    Color statusColor;

                    switch (status)
                    {
                        case "delivered":
                            statusColor = Color.FromArgb(39, 174, 96);
                            break;

                        case "in transit":
                            statusColor = Color.FromArgb(41, 128, 185);
                            break;

                        case "pending":
                            statusColor = Color.FromArgb(243, 156, 18);
                            break;

                        case "cancelled":
                            statusColor = Color.FromArgb(231, 76, 60);
                            break;

                        default:
                            statusColor = Color.Black;
                            break;
                    }
                    dgv.Rows[rowIndex].Cells["status"].Style.ForeColor = statusColor;
                    dgv.Rows[rowIndex].Cells["status"].Style.Font = new Font("Segoe UI Bold", 9);
                }
            }
        }

        private void CustomerTracking_Load(object sender, EventArgs e) { }
    }
}