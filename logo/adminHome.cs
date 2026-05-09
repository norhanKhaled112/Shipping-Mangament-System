using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class adminHome : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;

        public adminHome(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            SetupUI();
        }

        private void SetupUI()
        {
            this.Size = new Size(1030, 760);
            this.BackColor = bgGray;
            this.Text = "Admin Home";
            this.StartPosition = FormStartPosition.CenterScreen;

            // SIDEBAR
            Panel sidebar = new Panel { Width = 200, Dock = DockStyle.Left, BackColor = Color.White };
            this.Controls.Add(sidebar);

            PictureBox logo = new PictureBox { Size = new Size(160, 90), Location = new Point(20, 20), SizeMode = PictureBoxSizeMode.Zoom };
            try { logo.Image = Image.FromFile(@"C:\system-analysis\logo\logo\logo.png"); } catch { }
            sidebar.Controls.Add(logo);

            string[] items = { "Home", "Shipments", "Tracking", "Feedback", "Reports", "Logout" };
            for (int i = 0; i < items.Length; i++)
            {
                Button btn = new Button
                {
                    Text = "      " + items[i],
                    Size = new Size(200, 50),
                    Location = new Point(0, 130 + (i * 55)),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI Semibold", 10),
                    ForeColor = darkBlue,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;

                

                string itemName = items[i];
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(91, 158, 201); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
                if (itemName == "Logout") btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                if (itemName == "Shipments") btn.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
                if (itemName == "Tracking") btn.Click += (s, e) => { new trackingadmin(_employeeId).Show(); this.Hide(); };
                if (itemName == "Drivers") btn.Click += (s, e) => { new drivers(_employeeId).Show(); this.Hide(); };
                if (itemName == "Feedback") btn.Click += (s, e) => { new feedback(_employeeId).Show(); this.Hide(); };
                if (itemName == "Reports") btn.Click += (s, e) => { new reports(_employeeId).Show(); this.Hide(); };

                sidebar.Controls.Add(btn);
            }

            // NAVBAR
            Panel navbar = new Panel { Height = 80, Width = this.Width - 200, Location = new Point(200, 0), BackColor = mainBlue };
            this.Controls.Add(navbar);

            Label titleNav = new Label
            {
                Text = "Wasalha-Shipping Management System",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 14),
                AutoSize = false,
                Size = new Size(navbar.Width, navbar.Height),
                TextAlign = ContentAlignment.MiddleCenter
            };
            navbar.Controls.Add(titleNav);

            // MAIN AREA
            Panel mainArea = new Panel
            {
                Location = new Point(200, 80),
                Size = new Size(this.Width - 200, this.Height - 80),
                BackColor = bgGray
            };
            this.Controls.Add(mainArea);

            // WELCOME
            Label welcome = new Label
            {
                Text = "Welcome, Admin 👋",
                Font = new Font("Segoe UI Bold", 18),
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = darkBlue
            };
            mainArea.Controls.Add(welcome);

            Label desc = new Label
            {
                Text = "Manage your shipping system easily",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 65),
                AutoSize = true,
                ForeColor = Color.Gray
            };
            mainArea.Controls.Add(desc);

            // CARDS
            int totalShipments = GetCount("SELECT COUNT(*) FROM SHIPMENT");
            int pending = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='pending'");
            int inTransit = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='in transit'");
            int delivered = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='delivered'");

            string[] cardLabels = { "Total Shipments", "Pending", "In Transit", "Delivered" };
            string[] cardValues = { totalShipments.ToString(), pending.ToString(), inTransit.ToString(), delivered.ToString() };
            string[] cardIcons = { "📦", "⏳", "🚚", "✅" };
            Color[] cardColors = {
                mainBlue,
                Color.FromArgb(243, 156, 18),
                Color.FromArgb(41, 128, 185),
                Color.FromArgb(39, 174, 96)
            };

            for (int i = 0; i < cardLabels.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(170, 100),
                    Location = new Point(30 + (i * 190), 110),
                    BackColor = Color.White
                };

                card.Controls.Add(new Label { Text = cardIcons[i], Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI Emoji", 16) });
                card.Controls.Add(new Label { Text = cardValues[i], Location = new Point(10, 45), AutoSize = true, Font = new Font("Segoe UI Bold", 20), ForeColor = cardColors[i] });
                card.Controls.Add(new Label { Text = cardLabels[i], Location = new Point(10, 75), AutoSize = true, Font = new Font("Segoe UI", 8), ForeColor = Color.Gray });

                mainArea.Controls.Add(card);
            }

            // BUTTONS
            Button btnViewShipment = new Button
            {
                Text = "View Shipments",
                Size = new Size(180, 45),
                Location = new Point(30, 240),
                BackColor = mainBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10),
                Cursor = Cursors.Hand
            };
            btnViewShipment.FlatAppearance.BorderSize = 0;
            btnViewShipment.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
            mainArea.Controls.Add(btnViewShipment);

            Button btnTrackShipment = new Button
            {
                Text = "Track Shipments",
                Size = new Size(180, 45),
                Location = new Point(225, 240),
                BackColor = Color.White,
                ForeColor = darkBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10),
                Cursor = Cursors.Hand
            };
            btnTrackShipment.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnTrackShipment.Click += (s, e) => { new trackingadmin(_employeeId).Show(); this.Hide(); };
            mainArea.Controls.Add(btnTrackShipment);
        }

        private int GetCount(string query)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
            }
        }

        private void adminHome_Load(object sender, EventArgs e) { }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void adminHome_Load_1(object sender, EventArgs e)
        {

        }
    }
}