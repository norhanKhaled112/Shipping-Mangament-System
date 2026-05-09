using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class home : Form
    {
        Panel sideBar;
        Button currentActiveButton;
        private int _customerId;

        public home(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            SetupUI();
        }

        private int GetShipmentCount()
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM SHIPMENT WHERE customer_id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", _customerId);
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
            }
        }

        void SetupUI()
        {
            this.Text = "Wasalha - Home";
            this.Size = new Size(1030, 760);
            this.BackColor = ColorTranslator.FromHtml("#F5F5F5");
            this.Font = new Font("Segoe UI", 10);

            // TOP PANEL
            Panel topPanel = new Panel();
            topPanel.BackColor = ColorTranslator.FromHtml("#4FA3D1");
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;
            this.Controls.Add(topPanel);

            Label title = new Label();
            title.Text = "Wasalha Shipping Management System";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI semibold", 16, FontStyle.Bold);
            title.Location = new Point(200, 25);
            title.AutoSize = true;
            topPanel.Controls.Add(title);

           
            // SIDEBAR
            sideBar = new Panel();
            sideBar.BackColor = Color.White;
            sideBar.Dock = DockStyle.Left;
            sideBar.Width = 200;
            this.Controls.Add(sideBar);

            PictureBox logo = new PictureBox();
            try { logo.Image = Image.FromFile(@"C:\system-analysis\logo\logo\logo.png"); } catch { }
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Location = new Point(25, 10);
            logo.Size = new Size(150, 100);
            sideBar.Controls.Add(logo);

            Button btnHome = CreateMenuButton("  Home", 130);
            Button btnShipment = CreateMenuButton("  Shipment", 185);
            Button btnTracking = CreateMenuButton("  Tracking", 240);
            Button btnProfile = CreateMenuButton("  Profile", 295);
            Button btnLogout = CreateMenuButton("  Logout", 350);
            Button btnHelp = CreateMenuButton("  Help", 405);

            btnLogout.ForeColor = Color.FromArgb(24, 54, 73);
            btnLogout.Click += (s, e) => { new login().Show(); this.Hide(); };
            btnShipment.Click += (s, e) => { new clientship(_customerId).Show(); this.Hide(); };
            btnTracking.Click += (s, e) => { new trackingclient(_customerId).Show(); this.Hide(); };
            btnProfile.Click += (s, e) => { new profile(_customerId).Show(); this.Hide(); };
            btnHelp.Click += (s, e) => { new help(_customerId).Show(); this.Hide(); };

            sideBar.Controls.Add(btnHome);
            sideBar.Controls.Add(btnShipment);
            sideBar.Controls.Add(btnTracking);
            sideBar.Controls.Add(btnProfile);
            sideBar.Controls.Add(btnLogout);
            sideBar.Controls.Add(btnHelp);

            // WELCOME
            Label welcome = new Label();
            welcome.Text = "Welcome to Wasalha 🚚";
            welcome.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            welcome.ForeColor = Color.FromArgb(24, 54, 73);
            welcome.Location = new Point(250, 100);
            welcome.AutoSize = true;
            this.Controls.Add(welcome);

            Label desc = new Label();
            desc.Text = "We are one of the leading shipping companies, providing fast, secure, and reliable delivery services.";
            desc.Font = new Font("Segoe UI Semibold", 10);
            desc.ForeColor = Color.FromArgb(24, 54, 73);
            desc.Size = new Size(600, 60);
            desc.Location = new Point(250, 140);
            this.Controls.Add(desc);

            // CENTER PANEL
            Panel centerPanel = new Panel();
            centerPanel.Size = new Size(400, 350);
            centerPanel.Location = new Point(
                (this.ClientSize.Width - centerPanel.Width) / 2,
                (this.ClientSize.Height - centerPanel.Height) / 2 + 50
            );
            centerPanel.BackColor = Color.Transparent;
            this.Controls.Add(centerPanel);

            // CARD
            Panel card = new Panel();
            card.Size = new Size(300, 180);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Location = new Point(80, 0);

            Label icon = new Label();
            icon.Text = "🚚";
            icon.Font = new Font("Segoe UI Emoji", 28);
            icon.AutoSize = true;
            icon.Location = new Point(120, 10);

            Label number = new Label();
            number.Text = GetShipmentCount().ToString();
            number.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            number.ForeColor = Color.FromArgb(24, 54, 73);
            number.AutoSize = true;
            number.Location = new Point(120, 55);

            Label txt = new Label();
            txt.Text = "Total Shipments";
            txt.Font = new Font("Segoe UI", 12);
            txt.ForeColor = Color.Gray;
            txt.AutoSize = true;
            txt.Location = new Point(75, 120);

            card.Controls.Add(icon);
            card.Controls.Add(number);
            card.Controls.Add(txt);

            // BUTTONS
            Button btn1 = new Button();
            btn1.Text = "Create Shipment";
            btn1.Size = new Size(150, 40);
            btn1.BackColor = ColorTranslator.FromHtml("#4FA3D1");
            btn1.ForeColor = Color.White;
            btn1.FlatStyle = FlatStyle.Flat;
            btn1.Location = new Point(80, 200);
            btn1.Click += (s, e) => { new clientship(_customerId).Show(); this.Hide(); };

            Button btn2 = new Button();
            btn2.Text = "Track Shipment";
            btn2.Size = new Size(150, 40);
            btn2.FlatStyle = FlatStyle.Flat;
            btn2.ForeColor = Color.FromArgb(24, 54, 73);
            btn2.Location = new Point(250, 200);
            btn2.Click += (s, e) => { new trackingclient(_customerId).Show(); this.Hide(); };

            centerPanel.Controls.Add(card);
            centerPanel.Controls.Add(btn1);
            centerPanel.Controls.Add(btn2);
        }

        Button CreateMenuButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 200;
            btn.Height = 50;
            btn.Top = top;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe MDL2 Assets", 12, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(24, 54, 73);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => {
                if (btn != currentActiveButton) btn.BackColor = Color.FromArgb(91, 158, 201) ;
            };
            btn.MouseLeave += (s, e) => {
                if (btn != currentActiveButton) btn.BackColor = Color.White;
            };
            btn.Click += (s, e) => {
                if (currentActiveButton != null) currentActiveButton.BackColor = Color.White;
                currentActiveButton = btn;
                btn.BackColor = ColorTranslator.FromHtml("#D6EAF8");
            };

            return btn;
        }

        private void home_Load(object sender, EventArgs e) { }
    }
}