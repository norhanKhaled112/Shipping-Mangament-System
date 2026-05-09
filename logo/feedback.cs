using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class feedback : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;

        public feedback(int employeeId)
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

            string[] items = { "Home", "Shipment", "Tracking", "Reports", "Feedback", "Logout" };
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
                };//-------------------
                btn.FlatAppearance.BorderSize = 0;
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb( 91, 158, 201); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };

                string itemName = items[i];
                if (itemName == "Logout")
                    btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                else if (itemName == "Home")
                    btn.Click += (s, e) => { new adminHome(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Shipment")
                    btn.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
                else if(itemName == "Tracking")
                    btn.Click += (s, e) => { new trackingadmin(_employeeId).Show(); this.Hide(); };
                else if (itemName == "Reports")
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
                Text = "Customer Feedback",
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
            int total = GetCount("SELECT COUNT(*) FROM SUPPORT_TICKET");
            int pending = GetCount("SELECT COUNT(*) FROM SUPPORT_TICKET WHERE status='pending'");
            int resolved = GetCount("SELECT COUNT(*) FROM SUPPORT_TICKET WHERE status='resolved'");

            string[] cardT = { "Total", "Pending", "Resolved" };
            string[] cardV = { total.ToString(), pending.ToString(), resolved.ToString() };

            for (int i = 0; i < cardT.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(160, 80),
                    Location = new Point(20 + (i * 175), 20),
                    BackColor = Color.White
                };

                card.Controls.Add(new Label
                {
                    Text = cardT[i],
                    Location = new Point(10, 10),
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.Gray,
                    AutoSize = true
                });

                card.Controls.Add(new Label
                {
                    Text = cardV[i],
                    Location = new Point(10, 35),
                    Font = new Font("Segoe UI Bold", 14),
                    ForeColor = mainBlue,
                    AutoSize = true
                });

                mainArea.Controls.Add(card);
            }

            // TABLE
            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(770, 450),
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

            dgv.Columns.Add("ticket_id", "ID");
            dgv.Columns.Add("customer", "Customer");
            dgv.Columns.Add("phone", "Phone");
            dgv.Columns.Add("subject", "Subject");
            dgv.Columns.Add("message", "Message");
            dgv.Columns.Add("status", "Status");
            dgv.Columns.Add("date", "Date");
            dgv.ReadOnly = true;
            mainArea.Controls.Add(dgv);
            LoadTickets(dgv);

            // زرار Mark as Resolved
            Button btnResolve = new Button
            {
                Text = "Mark as Resolved",
                Size = new Size(180, 40),
                Location = new Point(20, 585),
                BackColor = mainBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnResolve.FlatAppearance.BorderSize = 0;

            btnResolve.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int ticketId = Convert.ToInt32(dgv.SelectedRows[0].Cells["ticket_id"].Value);
                    using (SqlConnection conn = DATABASE.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE SUPPORT_TICKET SET status='resolved' WHERE ticket_id=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", ticketId);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Updated successfully!");
                    dgv.Rows.Clear();
                    LoadTickets(dgv);
                }
                else
                {
                    MessageBox.Show("Please select a ticket first!");
                }
            };

            mainArea.Controls.Add(btnResolve);
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

        private void LoadTickets(DataGridView dgv)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    t.ticket_id,
                                    c.name,
                                    c.phone,
                                    t.subject,
                                    t.message,
                                    t.status,
                                    t.created_at
                                 FROM SUPPORT_TICKET t
                                 JOIN CUSTOMER c ON t.customer_id = c.customer_id
                                 ORDER BY t.created_at DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgv.Rows.Add(
                        reader["ticket_id"].ToString(),
                        reader["name"].ToString(),
                        reader["phone"].ToString(),
                        reader["subject"].ToString(),
                        reader["message"].ToString(),
                        reader["status"].ToString(),
                        Convert.ToDateTime(reader["created_at"]).ToString("d MMM yyyy")
                    );
                }
            }
        }

        private void adminFeedback_Load(object sender, EventArgs e) { }
    }
}