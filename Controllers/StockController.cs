using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;

namespace PMS.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            List<Stock> stocklist = new();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM stock INNER JOIN medicine ON stock.stockmedicine_id = medicine.medicine_id;", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Stock stock = new();
                    stock.Id=Convert.ToInt32(reader["stock_id"]);
                    stock.MedicineID = Convert.ToInt32(reader["stockmedicine_id"]);
                    stock.Quantity= Convert.ToInt32(reader["quantity"]);
                    stock.Capacity = Convert.ToInt32(reader["capacity"]);
                    stock.MedicineName = reader["medicine_name"].ToString();
                    stocklist.Add(stock);
                }
                reader.Close();
            }
            return View(stocklist);
        }
    }
}
