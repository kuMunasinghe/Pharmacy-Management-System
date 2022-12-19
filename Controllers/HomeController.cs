using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
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
    }
}
