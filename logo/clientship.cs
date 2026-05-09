using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace wasalha
{
    public partial class clientship : Form
    {
        private readonly Color primaryBlue = Color.FromArgb(91, 158, 201);
        private readonly Color darkBlue = Color.FromArgb(24, 54, 73);
        private readonly Color backgroundColor = Color.FromArgb(245, 245, 245);
        private int _customerId;

        private TextBox txtSenderName, txtSenderPhone, txtSenderAddress, txtSenderCOD;
        private TextBox txtReceiverName, txtReceiverPhone, txtReceiverAddress;
        private ComboBox cmbCity, cmbType;
        private TextBox txtWeight, txtNotes;
        private RadioButton rbCash, rbVisa;
        private Label lblPrice;

        public clientship(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            SetupCustomDesign();
        }

        private void SetupCustomDesign()
        {
            this.Size = new Size(800, 760);
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI Semibold", 10);
            this.StartPosition = FormStartPosition.CenterScreen;

            // NAVBAR
            Panel topNav = new Panel { Height = 60, BackColor = primaryBlue, Dock = DockStyle.Top };
            Label lblTitle = new Label { Text = "Create New Shipment", ForeColor = Color.White, Location = new Point(230, 15), AutoSize = true, Font = new Font("Segoe UI Semibold", 15) };
            topNav.Controls.Add(lblTitle);
            this.Controls.Add(topNav);

            // SIDEBAR
            Panel sideBar = new Panel { Width = 200, BackColor = Color.White, Dock = DockStyle.Left };
            this.Controls.Add(sideBar);

            PictureBox logo = new PictureBox { Size = new Size(160, 90), Location = new Point(25, 10), SizeMode = PictureBoxSizeMode.Zoom };
            try { logo.Image = Image.FromFile(@"C:\system-analysis\logo\logo\Logo.png"); } catch { }
            sideBar.Controls.Add(logo);

            string[] menus = { " Home", "  Shipment", "  Tracking", " Profile", " Logout", "  Help" };
            for (int i = 0; i < menus.Length; i++)
            {
                Button btn = new Button
                {
                    Text = menus[i],
                    Size = new Size(210, 50),
                    Location = new Point(0, 120 + (i * 55)),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(20, 0, 0, 0),
                    ForeColor = Color.FromArgb(24, 54, 73),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.MouseEnter += (s, e) => { btn.BackColor = primaryBlue; btn.ForeColor = Color.White; };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; btn.ForeColor = Color.FromArgb(24, 54, 73); };

                string menuName = menus[i].Trim();
                if (menuName == "Home") btn.Click += (s, e) => { new home(_customerId).Show(); this.Hide(); };
                if (menuName == "Tracking") btn.Click += (s, e) => { new trackingclient(_customerId).Show(); this.Hide(); };
                if (menuName == "Profile") btn.Click += (s, e) => { new profile(_customerId).Show(); this.Hide(); };
                if (menuName == "Logout") btn.Click += (s, e) => { new login().Show(); this.Hide(); };
                if (menuName == "Help") btn.Click += (s, e) => { new help(_customerId).Show(); this.Hide(); };

                sideBar.Controls.Add(btn);
            }

            // SENDER INFORMATION
            GroupBox gbSender = CreateSection("SENDER INFORMATION", 220, 75, 760, 155);
            txtSenderName = AddInput(gbSender, "Name:", 20, 40, 150);
            txtSenderPhone = AddInput(gbSender, "Phone:", 20, 90, 150);
            txtSenderAddress = AddInput(gbSender, "Address:", 300, 40, 200);
            txtSenderCOD = AddInput(gbSender, "Product Price:", 300, 85, 150);

            // RECEIVER INFORMATION
            GroupBox gbReceiver = CreateSection("RECEIVER INFORMATION", 220, 240, 760, 160);
            txtReceiverName = AddInput(gbReceiver, "Name:", 20, 40, 150);
            txtReceiverPhone = AddInput(gbReceiver, "Phone:", 300, 40, 200);
            cmbCity = AddCombo(gbReceiver, "City:", 20, 95, 150);
            cmbCity.Items.AddRange(new string[] { "Cairo", "Alexandria", "Mansoura", "Giza", "Tanta" });
            txtReceiverAddress = AddInput(gbReceiver, "Address:", 300, 95, 200);

            // SHIPMENT DETAILS
            GroupBox gbDetails = CreateSection("SHIPMENT DETAILS", 220, 410, 760, 100);
            txtWeight = AddInput(gbDetails, "Weight:", 20, 45, 60);
            Label lblKg = new Label { Text = "kg", Location = new Point(190, 48), ForeColor = darkBlue, AutoSize = true };
            gbDetails.Controls.Add(lblKg);
            cmbType = AddCombo(gbDetails, "Type:", 220, 45, 150);
            cmbType.Items.AddRange(new string[] { "Documents", "Electronics", "Clothes", "Food", "Other" });
            txtNotes = AddInput(gbDetails, "Notes:", 490, 45, 150);

            // PAYMENT METHOD
            GroupBox gbPayment = CreateSection("PAYMENT METHOD", 220, 520, 760, 55);
            rbCash = new RadioButton { Text = "Cash 💵", Location = new Point(280, 18), Checked = true, AutoSize = true };
            rbVisa = new RadioButton { Text = "Visa 💳", Location = new Point(430, 18), AutoSize = true };
            gbPayment.Controls.AddRange(new Control[] { rbCash, rbVisa });

            // ESTIMATED PRICE
            Label lblPriceTitle = new Label
            {
                Text = "Estimated Price:",
                Location = new Point(220, 590),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 11),
                ForeColor = darkBlue
            };
            this.Controls.Add(lblPriceTitle);

            lblPrice = new Label
            {
                Text = "0 EGP",
                Location = new Point(380, 590),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 11),
                ForeColor = primaryBlue
            };
            this.Controls.Add(lblPrice);

            // CONFIRM BUTTON
            Button btnConfirm = new Button
            {
                Text = "Confirm Shipment",
                Size = new Size(300, 45),
                Location = new Point(360, 630),
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += btnConfirm_Click;
            this.Controls.Add(btnConfirm);

            txtWeight.TextChanged += (s, e) => CalculatePrice();
            cmbCity.SelectedIndexChanged += (s, e) => CalculatePrice();
        }

        private void CalculatePrice()
        {
            try
            {
                double weight = string.IsNullOrEmpty(txtWeight.Text) ? 0 : Convert.ToDouble(txtWeight.Text);
                double cityFee = 0;

                switch (cmbCity.SelectedItem?.ToString())
                {
                    case "Cairo": cityFee = 20; break;
                    case "Alexandria": cityFee = 35; break;
                    case "Mansoura": cityFee = 30; break;
                    case "Giza": cityFee = 25; break;
                    case "Tanta": cityFee = 30; break;
                }

                double price = (weight * 10) + cityFee;
                lblPrice.Text = price + " EGP";
            }
            catch { lblPrice.Text = "0 EGP"; }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenderName.Text) || string.IsNullOrEmpty(txtReceiverName.Text) ||
                string.IsNullOrEmpty(txtWeight.Text) || cmbCity.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields!");
                return;
            }

            double price = Convert.ToDouble(lblPrice.Text.Replace(" EGP", ""));
            string paymentMethod = rbCash.Checked ? "cash" : "visa";
            decimal codAmount = string.IsNullOrEmpty(txtSenderCOD.Text) ? 0 : Convert.ToDecimal(txtSenderCOD.Text);

            using (SqlConnection conn = DATABASE.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO SHIPMENT 
                               (customer_id, status, pickup_address, delivery_address, 
                                cod_amount, shipping_fee, created_at, payment_method)
                               VALUES 
                               (@customerId, 'pending', @pickup, @delivery, 
                                @codAmount, @shippingFee, @createdAt, @paymentMethod)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@customerId", _customerId);
                cmd.Parameters.AddWithValue("@pickup", txtSenderAddress.Text);
                cmd.Parameters.AddWithValue("@delivery", txtReceiverAddress.Text);
                cmd.Parameters.AddWithValue("@codAmount", codAmount);
                cmd.Parameters.AddWithValue("@shippingFee", price);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@paymentMethod", paymentMethod);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Shipment created successfully!");
        }

        private GroupBox CreateSection(string title, int x, int y, int w, int h)
        {
            GroupBox gb = new GroupBox
            {
                Text = title,
                Location = new Point(x, y),
                Size = new Size(w, h),
                ForeColor = darkBlue,
                Font = new Font("Segoe UI Bold", 9)
            };
            this.Controls.Add(gb);
            return gb;
        }

        private TextBox AddInput(GroupBox gb, string label, int x, int y, int w, bool isCombo = false, int height = 25)
        {
            Label lbl = new Label { Text = label, Location = new Point(x, y), AutoSize = true, ForeColor = Color.Black };
            gb.Controls.Add(lbl);
            TextBox txt = new TextBox { Location = new Point(x + 100, y - 3), Width = w };
            gb.Controls.Add(txt);
            return txt;
        }

        private ComboBox AddCombo(GroupBox gb, string label, int x, int y, int w)
        {
            Label lbl = new Label { Text = label, Location = new Point(x, y), AutoSize = true, ForeColor = Color.Black };
            gb.Controls.Add(lbl);
            ComboBox cmb = new ComboBox { Location = new Point(x + 60, y - 3), Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
            gb.Controls.Add(cmb);
            return cmb;
        }

        private void clientship_Load(object sender, EventArgs e) { }
        private void Form5_Load(object sender, EventArgs e) { }
    }
}