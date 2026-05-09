using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class help : Form
    {
        Panel sideBar;
        Button currentActiveButton;
        private int _customerId;
        private readonly Color primaryBlue = Color.FromArgb(91, 158, 201);
        private readonly Color darkBlue = Color.FromArgb(24, 54, 73);
        private readonly Color backgroundColor = Color.FromArgb(245, 245, 245);

        public help(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            SetupUI();
        }

        void SetupUI()
        {
            this.Text = "Help";
            this.Size = new Size(900, 760);
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 10);
            this.StartPosition = FormStartPosition.CenterScreen;

            // TOP PANEL
            Panel topPanel = new Panel();
            topPanel.BackColor = primaryBlue;
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;
            this.Controls.Add(topPanel);

            Label title = new Label();
            title.Text = "Wasalha Shipping Management System";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold);
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
            btnHome.Click += (s, e) => { new home(_customerId).Show(); this.Hide(); };
            btnShipment.Click += (s, e) => { new clientship(_customerId).Show(); this.Hide(); };
            btnTracking.Click += (s, e) => { new trackingclient(_customerId).Show(); this.Hide(); };
            btnProfile.Click += (s, e) => { new profile(_customerId).Show(); this.Hide(); };
            btnLogout.Click += (s, e) => { new login().Show(); this.Hide(); };

            sideBar.Controls.Add(btnHome);
            sideBar.Controls.Add(btnShipment);
            sideBar.Controls.Add(btnTracking);
            sideBar.Controls.Add(btnProfile);
            sideBar.Controls.Add(btnLogout);
            sideBar.Controls.Add(btnHelp);

            // MAIN AREA
            Panel mainArea = new Panel();
            mainArea.Location = new Point(200, 80);
            mainArea.Size = new Size(this.Width - 200, this.Height - 80);
            mainArea.BackColor = backgroundColor;
            this.Controls.Add(mainArea);

            Label lblTitle = new Label();
            lblTitle.Text = "Help & Support";
            lblTitle.Font = new Font("Segoe UI Bold", 14);
            lblTitle.ForeColor = darkBlue;
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            mainArea.Controls.Add(lblTitle);

            // CONTACT SUPPORT PANEL
            Panel contactPanel = new Panel();
            contactPanel.Location = new Point(20, 60);
            contactPanel.Size = new Size(760, 220);
            contactPanel.BackColor = Color.WhiteSmoke;
            contactPanel.BorderStyle = BorderStyle.FixedSingle;
            mainArea.Controls.Add(contactPanel);

            Label lblContact = new Label();
            lblContact.Text = "Contact Support";
            lblContact.Font = new Font("Segoe UI Bold", 11);
            lblContact.ForeColor = primaryBlue;
            lblContact.Location = new Point(10, 10);
            lblContact.AutoSize = true;
            contactPanel.Controls.Add(lblContact);

            Label lblSubject = new Label();
            lblSubject.Text = "Subject:";
            lblSubject.Location = new Point(30, 55);
            lblSubject.AutoSize = true;
            contactPanel.Controls.Add(lblSubject);

            TextBox txtSubject = new TextBox();
            txtSubject.Location = new Point(130, 52);
            txtSubject.Size = new Size(300, 30);
            contactPanel.Controls.Add(txtSubject);

            Label lblMessage = new Label();
            lblMessage.Text = "Message:";
            lblMessage.Location = new Point(30, 100);
            lblMessage.AutoSize = true;
            contactPanel.Controls.Add(lblMessage);

            TextBox txtMessage = new TextBox();
            txtMessage.Location = new Point(130, 97);
            txtMessage.Size = new Size(580, 80);
            txtMessage.Multiline = true;
            contactPanel.Controls.Add(txtMessage);

            Button btnSend = new Button();
            btnSend.Text = "Send message";
            btnSend.Size = new Size(130, 30);
            btnSend.Location = new Point(130, 185);
            btnSend.BackColor = primaryBlue;
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            contactPanel.Controls.Add(btnSend);

            Button btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Size = new Size(80, 30);
            btnClear.Location = new Point(270, 185);
            btnClear.BackColor = primaryBlue;
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 0;
            contactPanel.Controls.Add(btnClear);

            btnClear.Click += (s, e) => { txtSubject.Clear(); txtMessage.Clear(); };

            btnSend.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSubject.Text) || string.IsNullOrEmpty(txtMessage.Text))
                {
                    MessageBox.Show("Please enter both subject and message.");
                    return;
                }
                using (SqlConnection conn = DATABASE.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO SUPPORT_TICKET (customer_id, subject, message) 
                                     VALUES (@customerId, @subject, @message)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@customerId", _customerId);
                    cmd.Parameters.AddWithValue("@subject", txtSubject.Text);
                    cmd.Parameters.AddWithValue("@message", txtMessage.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("send successfully");
                txtSubject.Clear();
                txtMessage.Clear();
                LoadReplies(mainArea);
            };

            
        }

        private void LoadReplies(Panel mainArea)
        {
            
           

            
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT subject, message, status,  created_at 
                                 FROM SUPPORT_TICKET 
                                 WHERE customer_id = @id 
                                 ORDER BY created_at DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", _customerId);
                SqlDataReader reader = cmd.ExecuteReader();

                int yPos = 40;
                while (reader.Read())
                {
                    Panel ticketCard = new Panel();
                    ticketCard.Size = new Size(720, 75);
                    ticketCard.Location = new Point(10, yPos);
                    ticketCard.BackColor = Color.White;
                    ticketCard.BorderStyle = BorderStyle.FixedSingle;
                    

                    Label lblSubject = new Label();
                    lblSubject.Text = "📩 " + reader["subject"].ToString();
                    lblSubject.Font = new Font("Segoe UI Bold", 10);
                    lblSubject.ForeColor = darkBlue;
                    lblSubject.Location = new Point(10, 8);
                    lblSubject.AutoSize = true;
                    ticketCard.Controls.Add(lblSubject);

                    string status = reader["status"].ToString();
                    Label lblStatus = new Label();
                    lblStatus.Text = status;
                    lblStatus.Font = new Font("Segoe UI Bold", 9);
                    lblStatus.ForeColor = status == "resolved" ? Color.FromArgb(39, 174, 96) : Color.FromArgb(243, 156, 18);
                    lblStatus.Location = new Point(600, 8);
                    lblStatus.AutoSize = true;
                    ticketCard.Controls.Add(lblStatus);

                    Label lblMsg = new Label();
                    lblMsg.Text = "Message: " + reader["message"].ToString();
                    lblMsg.Font = new Font("Segoe UI", 9);
                    lblMsg.ForeColor = Color.Gray;
                    lblMsg.Location = new Point(10, 32);
                    lblMsg.Size = new Size(700, 20);
                    ticketCard.Controls.Add(lblMsg);

                   

                    yPos += 85;
                }
            }
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
                if (btn != currentActiveButton) btn.BackColor = Color.FromArgb(91, 158, 201);
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

        private void Help_Load(object sender, EventArgs e) { }
    }
}