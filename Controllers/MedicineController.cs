using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;

namespace PMS.Controllers
{
    public class MedicineController : Controller
    {
        public IActionResult Index()
        {

            List<Medicine> medlist = new();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM medicine INNER JOIN company ON medicine.manufacture_id = company.id;", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Medicine med = new Medicine();
                    med.Id=Convert.ToInt32(reader["medicine_id"]);
                    med.Name = reader["medicine_name"].ToString();
                    med.Manufacture_ID= Convert.ToInt32(reader["id"]);
                    med.Manufacture = reader["company_name"].ToString();
                    med.Unit = float.Parse(reader["unit_price"].ToString());
                    med.Type = reader["medicine_type"].ToString();
                    med.Description=reader["description"].ToString();
                  

                    medlist.Add(med);
                }
                reader.Close();
            }
            return View(medlist);
        }
    }
}
