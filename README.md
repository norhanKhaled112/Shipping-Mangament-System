# 🚚 Wasalha — Shipping Management System



![Status](https://img.shields.io/badge/Status-Complete-brightgreen)




![Platform](https://img.shields.io/badge/Platform-Windows-blue)




![Language](https://img.shields.io/badge/Language-C%23-purple)




![Database](https://img.shields.io/badge/Database-SQL%20Server-red)



> **Your delivery, our mission** 🚀

---

## 📌 About The Project

**Wasalha** is a Windows Forms desktop application designed to streamline shipping operations for a shipping company.  
It supports two types of users — **Customers** and **Admins** — each with their own dedicated dashboard and features.

---

## 📸 Screenshots

### 🚀 Splash Screen


![Splash](screenshots/splash.png)



### 👤 Customer Home


![Customer Home](screenshots/customer_home.png)



### 📦 Create Shipment


![Create Shipment](screenshots/create_shipment.png)



### 🚚 Customer Tracking


![Customer Tracking](screenshots/customer_tracking.png)



### 🛠️ Admin Home


![Admin Home](screenshots/admin_home.png)



### 📦 Admin Shipments


![Admin Shipments](screenshots/admin_shipments.png)



### 📍 Admin Tracking


![Admin Tracking](screenshots/admin_tracking.png)



### 📊 Reports & Analytics


![Reports](screenshots/reports.png)



---

## ✨ Features

### 👤 Customer
- 🔐 Register & Login
- 📦 Create new shipments with automatic price calculation
- 🚚 Track shipment status
- 👤 View personal profile
- 💬 Send support messages to admin

### 🛠️ Admin
- 📊 Dashboard with live statistics
- 📦 View & manage all shipments
- 🔄 Update shipment status
- 🚗 Manage drivers (Available / Busy)
- 💬 View & resolve customer feedback
- 📈 Reports & analytics with charts
- 💰 Revenue & COD tracking

---

## 🗄️ Database Tables

| Table | Description |
|-------|-------------|
| `CUSTOMER` | Customer information |
| `EMPLOYEE` | Admin information |
| `SHIPMENT` | All shipment records |
| `DRIVER` | Driver information & availability |
| `USERS` | Login credentials |
| `SUPPORT_TICKET` | Customer support messages |

---

## 🛠️ Tech Stack

| Technology | Usage |
|------------|-------|
| C# | Main programming language |
| Windows Forms | Desktop UI framework |
| SQL Server | Database |
| ADO.NET | Database connectivity |

---

## 🚀 How To Run

1. Clone the repository
```bash
git clone https://github.com/norhanKhaled112/Shipping-Mangament-System.git
```
2. Open the solution in **Visual Studio**
3. Run the SQL scripts to create the database
4. Update the connection string in `DBHelper.cs`
5. Build & Run ▶️

---

## 👥 Team

<table>
  <tr>
    <td align="center">
      <b>Norhan Khaled</b>
    </td>
    <td align="center">
      <b>Malak Mohamed</b>
    </td>
    <td align="center">
      <b>Kholoud Rashid</b>
    </td>
    <td align="center">
      <b>Basmala Ayman</b>
    </td>
  </tr>
</table>

---

## 📄 License

This project was built as an academic project — ITI 2026.

---

<p align="center">Made by Team Wasalha 🚚</p>
