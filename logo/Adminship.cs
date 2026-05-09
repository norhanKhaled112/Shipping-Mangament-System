using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class Adminship : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;

        public Adminship(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            this.Size = new Size(1030, 760);
            this.BackColor = bgGray;
            this.Text = "Wasalha System";
            this.StartPosition = FormStartPosition.CenterScreen;
            BuildFinalDesign();
        }

        private void BuildFinalDesign()
        {
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

            string[] items = { "Home", "Shipment", "Tracking","Reports", "Logout" };
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
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(91, 158, 201); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
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
               
                else if (itemName == "Reports")
                    btn.Click += (s, e) => { new reports(_employeeId).Show(); this.Hide(); };

                sidebar.Controls.Add(btn);
            }

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
                Text = "Shipments",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 16),
                AutoSize = false,
                Size = new Size(navbar.Width, navbar.Height),
                TextAlign = ContentAlignment.MiddleCenter
            };
            navbar.Controls.Add(title);

            Panel mainArea = new Panel
            {
                Location = new Point(200, 80),
                Size = new Size(this.Width - 200, this.Height - 80),
                BackColor = bgGray
            };
            this.Controls.Add(mainArea);

            string[] cardT = { "Total", "Pending", "In Transit", "Delivered", "Revenue" };
            string[] cardV = {
                GetCount("SELECT COUNT(*) FROM SHIPMENT").ToString(),
                GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='pending'").ToString(),
                GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='in transit'").ToString(),
                GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='delivered'").ToString(),
                GetSum("SELECT SUM(shipping_fee) FROM SHIPMENT") + " EGP"
            };

            for (int i = 0; i < cardT.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(140, 80),
                    Location = new Point(20 + (i * 155), 20),
                    BackColor = Color.White
                };

                card.Controls.Add(new Label
                {
                    Text = cardT[i],
                    Location = new Point(10, 10),
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.Gray
                });

                card.Controls.Add(new Label
                {
                    Text = cardV[i],
                    Location = new Point(10, 35),
                    Font = new Font("Segoe UI Bold", 11),
                    ForeColor = mainBlue,
                    AutoSize = true
                });

                mainArea.Controls.Add(card);
            }

            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(770, 500),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                EnableHeadersVisualStyles = false
            };

            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dgv.Columns.Add("id", "ID");
            dgv.Columns.Add("name", "Client");
            dgv.Columns.Add("from", "From");
            dgv.Columns.Add("to", "To");
            dgv.Columns.Add("price", "Price");
            
            dgv.Columns.Add("status", "Status");
            dgv.Columns.Add("date", "Date");
            dgv.ReadOnly = true;

            mainArea.Controls.Add(dgv);
            LoadShipments(dgv);
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

        private string GetSum(string query)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? "0" : Convert.ToDecimal(result).ToString("N0");
            }
        }

        private void LoadShipments(DataGridView dgv)
        {
            using (SqlConnection conn =DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    s.shipment_id,
                                    c.name,
                                    s.pickup_address,
                                    s.delivery_address,
                                    s.shipping_fee,
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
                        reader["pickup_address"].ToString(),
                        reader["delivery_address"].ToString(),
                        reader["shipping_fee"].ToString() + " EGP",
                       
                        reader["status"].ToString(),
                        Convert.ToDateTime(reader["created_at"]).ToString("d MMM yyyy")
                    );
                }
            }
        }

        private void Adminship_Load(object sender, EventArgs e) { }

        private void Adminship_Load_1(object sender, EventArgs e)
        {

        }
    }
}