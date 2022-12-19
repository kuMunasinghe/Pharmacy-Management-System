using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Routing.Constraints;

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

        public IActionResult Delete(int id)
        {
            string query = @"
                        delete from medicine 
                        where medicine_id=@id;
                        
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
                        TempData["Message"] = "Medicine Data Deleted !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Delete Failed !";
                        return RedirectToAction("Index");
                    }

                }
            }
        }

        public IActionResult Create()
        {
            return View();
        }

       
        public IActionResult CreateSubmit(Medicine medicine)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO `medicine` (`medicine_id`, `medicine_name`, `manufacture_id`, `unit_price`, `medicine_type`, `description`) 
                                     VALUES (NULL,@name, @manufacture_id,@unit,@type,@description);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", medicine.Name);
                    cmd.Parameters.AddWithValue("@manufacture_id",medicine.Manufacture_ID);
                    cmd.Parameters.AddWithValue("@unit", medicine.Unit);
                    cmd.Parameters.AddWithValue("@type", medicine.Type);
                    cmd.Parameters.AddWithValue("@description",medicine.Description);
                  
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Medicine Data Added Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Error !";
                            return RedirectToAction("Index");
                        }


                    }
                    catch (Exception ex)
                    {

                        TempData["Message"] = "Invalid Manufacture ID !";
                        return RedirectToAction("Index");

                    }
                }
                catch (AggregateException)
                {
                    TempData["Message"] = "Database connection Error !";
                    return RedirectToAction("Index");

                }

            }
        }


        public IActionResult Edit(int id)
        {
            TempData["ID"] = id;

            return View();  
        }

        public IActionResult Save(Medicine medicine)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"
                                          UPDATE medicine SET unit_price = @unit WHERE medicine.medicine_id = @id;
                                       ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", medicine.Id);
                    cmd.Parameters.AddWithValue("unit", medicine.Unit);
                 
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Data Edited Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Edit Failed !";
                            return RedirectToAction("Index");
                        }


                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = ex.Message;
                        return RedirectToAction("Index");


                    }
                }
                catch (AggregateException)
                {
                    TempData["Message"] = "Database Error !";
                    return RedirectToAction("Index");

                }

            }
        }
    }

}
