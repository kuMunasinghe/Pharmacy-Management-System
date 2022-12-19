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
                    try {
                        getretail.Tel = Convert.ToInt32(reader["retail_tel"]);
                    }
                    catch (Exception)
                    {
                        getretail.Tel = 0;
                    }
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
                        where retail_id=@id;
                        
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
                        TempData["Message"] = " Deleted !";
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

        public IActionResult CreateSubmit(Retail retail)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO retail (retail_id, retail_name, retail_type, retail_tel, retail_address) 
                                   VALUES (NULL,@name,@type,@tel,@add);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name",retail.Name);
                    cmd.Parameters.AddWithValue("@type", retail.Type);
                    cmd.Parameters.AddWithValue("@tel", retail.Tel);
                    cmd.Parameters.AddWithValue("@add", retail.Address);
                  

                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Retailer Added Successfully !";
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

                        TempData["Message"] = ex.Message+" !";
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

        public IActionResult Save(Retail ret)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"
                                    UPDATE retail SET retail_tel = @tel, retail_address = @add WHERE retail.retail_id = @id;
                                       ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", ret.Id);
                    cmd.Parameters.AddWithValue("tel", ret.Tel);
                    cmd.Parameters.AddWithValue("add", ret.Address);

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
