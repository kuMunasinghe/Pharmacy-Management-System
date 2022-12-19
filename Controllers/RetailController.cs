using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;

namespace PMS.Controllers
{
    public class RetailController : Controller
    {
        public IActionResult Index()
        {

            List<Retail> retailer = new();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from retail", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Retail getretail = new();
                    getretail.Id = Convert.ToInt32(reader["retail_id"]);
                    getretail.Name = reader["retail_name"].ToString();
                    getretail.Type = reader["retail_type"].ToString();
                    getretail.Tel = Convert.ToInt32(reader["retail_tel"]);
                    getretail.Address = reader["retail_address"].ToString();
                    retailer.Add(getretail);
                }
                reader.Close();
            }
            return View(retailer);
        }
    }
}
