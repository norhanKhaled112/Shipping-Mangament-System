using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace wasalha
{
    public partial class reports : Form
    {
        Color mainBlue = Color.FromArgb(91, 158, 201);
        Color darkBlue = Color.FromArgb(24, 54, 73);
        Color bgGray = Color.FromArgb(245, 245, 245);
        private int _employeeId;

        public reports(int employeeId)
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
            Panel sidebar = new Panel { Width = 200, Dock = DockStyle.Left, BackColor = Color.White };
            this.Controls.Add(sidebar);

            PictureBox logo = new PictureBox { Size = new Size(160, 90), Location = new Point(20, 20), SizeMode = PictureBoxSizeMode.Zoom };
            try { logo.Image = Image.FromFile(@"C:\system-analysis\logo\logo\Logo.png"); } catch { }
            sidebar.Controls.Add(logo);

            string[] items = { "Home", "Shipment", "Tracking",  "Reports", "Feedback", "Logout" };
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
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(91, 158, 201); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
                string itemName = items[i];
               
                if (itemName == "Logout") btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                if (itemName == "Home") btn.Click += (s, e) => { new adminHome(_employeeId).Show(); this.Hide(); };
                if (itemName == "Shipment") btn.Click += (s, e) => { new Adminship(_employeeId).Show(); this.Hide(); };
                if (itemName == "Tracking") btn.Click += (s, e) => { new trackingadmin(_employeeId).Show(); this.Hide(); };
                
                if (itemName == "Feedback") btn.Click += (s, e) => { new feedback(_employeeId).Show(); this.Hide(); };

                sidebar.Controls.Add(btn);
            }
            

            // NAVBAR
            Panel navbar = new Panel { Height = 80, Width = this.Width - 200, Location = new Point(200, 0), BackColor = mainBlue };
            this.Controls.Add(navbar);
            navbar.Controls.Add(new Label
            {
                Text = "Reports & Analytics",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 16),
                AutoSize = false,
                Size = new Size(navbar.Width, navbar.Height),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // MAIN AREA
            Panel mainArea = new Panel
            {
                Location = new Point(200, 80),
                Size = new Size(this.Width - 200, this.Height - 80),
                BackColor = bgGray
            };
            this.Controls.Add(mainArea);

            // جيب البيانات
            int totalShipments = GetCount("SELECT COUNT(*) FROM SHIPMENT");
            int pending = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='pending'");
            int inTransit = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='in transit'");
            int delivered = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='delivered'");
            int cancelled = GetCount("SELECT COUNT(*) FROM SHIPMENT WHERE status='cancelled'");
            decimal totalRevenue = GetDecimal("SELECT ISNULL(SUM(shipping_fee), 0) FROM SHIPMENT WHERE status='delivered'");
            decimal totalCOD = GetDecimal("SELECT ISNULL(SUM(cod_amount), 0) FROM SHIPMENT WHERE status='delivered'");
            decimal pendingCOD = GetDecimal("SELECT ISNULL(SUM(cod_amount), 0) FROM SHIPMENT WHERE status='in transit'");

            // CARDS
            string[] cardLabels = { "Total", "Pending", "In Transit", "Delivered", "Cancelled" };
            string[] cardValues = { totalShipments.ToString(), pending.ToString(), inTransit.ToString(), delivered.ToString(), cancelled.ToString() };
            Color[] cardColors = {
                mainBlue,
                Color.FromArgb(243, 156, 18),
                Color.FromArgb(41, 128, 185),
                Color.FromArgb(39, 174, 96),
                Color.FromArgb(231, 76, 60)
            };

            for (int i = 0; i < cardLabels.Length; i++)
            {
                Panel card = new Panel
                {
                    Size = new Size(140, 80),
                    Location = new Point(10 + (i * 152), 10),
                    BackColor = Color.White
                };
                card.Controls.Add(new Label { Text = cardLabels[i], Location = new Point(10, 8), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray });
                card.Controls.Add(new Label { Text = cardValues[i], Location = new Point(10, 32), AutoSize = true, Font = new Font("Segoe UI Bold", 18), ForeColor = cardColors[i] });
                mainArea.Controls.Add(card);
            }

            // MONEY CARDS
            Panel cardRevenue = CreateMoneyCard("Total Revenue", totalRevenue + " EGP", Color.FromArgb(39, 174, 96), 10, 105);
            Panel cardCOD = CreateMoneyCard("Cash on delivery Collected", totalCOD + " EGP", mainBlue, 270, 105);
            Panel cardPending = CreateMoneyCard("Pending COD", pendingCOD + " EGP", Color.FromArgb(243, 156, 18), 530, 105);
            mainArea.Controls.Add(cardRevenue);
            mainArea.Controls.Add(cardCOD);
            mainArea.Controls.Add(cardPending);

            // CHART - Shipments by Status
            Chart chart = new Chart
            {
                Location = new Point(10, 205),
                Size = new Size(380, 300),
                BackColor = Color.White
            };

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = Color.White;
            chart.ChartAreas.Add(chartArea);

            Series series = new Series { ChartType = SeriesChartType.Pie };
            series.Points.AddXY("Pending", pending);
            series.Points.AddXY("In Transit", inTransit);
            series.Points.AddXY("Delivered", delivered);
            series.Points.AddXY("Cancelled", cancelled);
            

            series.Points[0].Color = Color.FromArgb(243, 156, 18);
            series.Points[1].Color = Color.FromArgb(41, 128, 185);
            series.Points[2].Color = Color.FromArgb(39, 174, 96);
            series.Points[3].Color = Color.FromArgb(231, 76, 60);

            series["PieLabelStyle"] = "Disabled";
            chart.Series.Add(series);

            Legend legend = new Legend { BackColor = Color.White };
            chart.Legends.Add(legend);

            Title chartTitle = new Title { Text = "Shipments by Status", Font = new Font("Segoe UI Semibold", 10), ForeColor = darkBlue };
            chart.Titles.Add(chartTitle);

            mainArea.Controls.Add(chart);

            // CHART - Revenue Bar
            Chart barChart = new Chart
            {
                Location = new Point(410, 205),
                Size = new Size(380, 300),
                BackColor = Color.White
            };

            ChartArea barArea = new ChartArea();
            barArea.BackColor = Color.White;
            barArea.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            barArea.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            barChart.ChartAreas.Add(barArea);

            Series barSeries = new Series { ChartType = SeriesChartType.Bar };
            barSeries.Points.AddXY("Revenue", (double)totalRevenue);
            barSeries.Points.AddXY("COD Collect", (double)totalCOD);
            barSeries.Points.AddXY("Pending COD", (double)pendingCOD);

            barSeries.Points[0].Color = Color.FromArgb(39, 174, 96);
            barSeries.Points[1].Color = mainBlue;
            barSeries.Points[2].Color = Color.FromArgb(243, 156, 18);

            barChart.Series.Add(barSeries);

            Title barTitle = new Title { Text = "Financial Overview (EGP)", Font = new Font("Segoe UI Semibold", 10), ForeColor = darkBlue };
            barChart.Titles.Add(barTitle);

            mainArea.Controls.Add(barChart);
        }

        private Panel CreateMoneyCard(string label, string value, Color color, int x, int y)
        {
            Panel card = new Panel { Size = new Size(240, 70), Location = new Point(x, y), BackColor = Color.White };
            card.Controls.Add(new Label { Text = label, Location = new Point(10, 8), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray });
            card.Controls.Add(new Label { Text = value, Location = new Point(10, 30), AutoSize = true, Font = new Font("Segoe UI Bold", 13), ForeColor = color });
            return card;
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

        private decimal GetDecimal(string query)
        {
            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value || result == null ? 0 : Convert.ToDecimal(result);
            }
        }

        private void adminReports_Load(object sender, EventArgs e) { }
    }
}