using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;

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

        public IActionResult Delete(int id)
        {
            string query = @"
                        delete from retail 
                        where id=@id;
                        
            ";

            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    int status = myReader.RecordsAffected;
                    myReader.Close();

                    mycon.Close();
                    if (status == 1)
                    {
                        TempData["Message"] = "User Deleted !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "User Delete Failed !";
                        return RedirectToAction("Index");
                    }

                }
            }
        }
    }
}
