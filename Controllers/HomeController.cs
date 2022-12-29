using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace PMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Auth(string email,string pwd)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT * FROM users WHERE email=@email AND password=@pwd";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("pwd", pwd);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    List<Users> users = new List<Users>();
                    //GET DATA
                    Users user = new Users();
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.First_Name = reader["first_name"].ToString();
                    user.Last_Name = reader["last_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Mobile = Convert.ToInt32(reader["mobile"]);
                    user.Address = reader["address"].ToString();
                    if (user.Email == email && user.Password == pwd)
                    {
                        string session_value = user.Email;
                        return RedirectToAction("Home");             
                            
                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return RedirectToAction("Index");
        }

        public IActionResult Home()
        {
            DashboardData dashdata = new();
            dashdata.SaleCount = getSaleCount();
            dashdata.StockCount = getStockCount();
            dashdata.RetailCount = getRetailCount();
            dashdata.Company = getCompanyCount();
            dashdata.RegisteredMed = getMedCount();
            dashdata.UserCount = getUserCount();
            return View(dashdata);
        }
        public IActionResult UserManager()
        {
            List<Users> users = new List<Users>();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from users",conn);
                MySqlDataReader reader=cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Users user = new Users();
                    user.Id=Convert.ToInt32(reader["id"]);
                    user.First_Name = reader["first_name"].ToString();
                    user.Last_Name= reader["last_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Mobile= Convert.ToInt32(reader["mobile"]);
                    user.Address = reader["address"].ToString(); 
                    
                    users.Add(user);
                }
                reader.Close();
            }
                return View(users);
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int getSaleCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(sales.sale_id) as salecount FROM sales ;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                   
                    //GET DATA
                    DashboardData sale = new ();
                    sale.SaleCount = Convert.ToInt32(reader["salecount"]);
                  
                    if (sale != null)
                    {
                        return sale.SaleCount;

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return 0;
        }

        public int getStockCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(stock.stock_id) as stockcount FROM stock ;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //GET DATA
                    DashboardData data = new();
                    data.SaleCount = Convert.ToInt32(reader["stockcount"]);

                    if (data != null)
                    {
                        return data.SaleCount;

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return 0;
        }

        public int getRetailCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(retail.retail_id) as retailcount FROM retail ;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //GET DATA
                    DashboardData data = new();
                    data.SaleCount = Convert.ToInt32(reader["retailcount"]);

                    if (data != null)
                    {
                        return data.SaleCount;

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return 0;
        }

        public int getCompanyCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(company.id) as companycount FROM company;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //GET DATA
                    DashboardData data = new();
                    data.SaleCount = Convert.ToInt32(reader["companycount"]);

                    if (data != null)
                    {
                        return data.SaleCount;

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return 0;
        }

        public int getMedCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(medicine.medicine_id) as medcount FROM medicine ;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //GET DATA
                    DashboardData data = new();
                    data.SaleCount = Convert.ToInt32(reader["medcount"]);

                    if (data != null)
                    {
                        return data.SaleCount;

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "User Email or Password is Incorrect !";
            return 0;
        }

        public int getUserCount()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT COUNT(id) AS usercount FROM users;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //GET DATA
                    DashboardData data = new();
                    data.UserCount = Convert.ToInt32(reader["usercount"]);

                    if (data != null)
                    {
                        return data.UserCount;

                    }

                }
                reader.Close();
            }
      
            return 0;
        }
    }
}
