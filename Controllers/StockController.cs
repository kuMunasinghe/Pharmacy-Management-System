using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;

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

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Stock stock)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO stock (stock_id, stockmedicine_id, quantity, capacity) 
                                     VALUES (NULL,@smid,@quan,@capa);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@smid", stock.MedicineID);
                    cmd.Parameters.AddWithValue("@quan", stock.Quantity);
                    cmd.Parameters.AddWithValue("@capa", stock.Capacity);
                 
                    

                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Stock Data Added Successfully !";
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

                        TempData["Message"] = ex.Message+"!";
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

        public IActionResult Save(Stock st)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"
                                     UPDATE stock SET quantity = @quan WHERE stock.stock_id = @id;
                                       ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", st.Id);
                    cmd.Parameters.AddWithValue("quan", st.Quantity);

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

        public IActionResult Delete(int id)
        {
            string query = @"
                        delete from stock 
                        where stock_id=@id;
                        
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
                        TempData["Message"] = "Stock Record Deleted !";
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
    }
}
