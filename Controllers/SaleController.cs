using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Data;

namespace PMS.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult Index()
        {

            List<Sales> sales = new List<Sales>();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sales", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Sales getsale = new Sales();
                    getsale.sale_id = Convert.ToInt32(reader["sale_id"]);

                   var basicdate = Convert.ToDateTime(reader["sale_date"]);
                    getsale.sale_date = basicdate.Date.ToString("dd/MM/yyyy");
                    getsale.sale_name = reader["sale_name"].ToString();
                    getsale.sale_type = reader["sale_type"].ToString();
                    getsale.sale_value = Convert.ToInt32(reader["sale_value"]);
                    getsale.sale_description = reader["sale_description"].ToString();


                    sales.Add(getsale);
                }
                reader.Close();
            }
            return View(sales);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateSubmit(Sales saledetails)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO `sales` (`sale_id`, `sale_date`, `sale_name`, `sale_type`, `sale_value`, `sale_description`) 
                                    VALUES (NULL, @sale_date, @sale_name,@sale_type,@sale_value,@sale_description)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("sale_id", saledetails.sale_id);
                    cmd.Parameters.AddWithValue("sale_date",saledetails.sale_date);
                    cmd.Parameters.AddWithValue("sale_name", saledetails.sale_name);
                    cmd.Parameters.AddWithValue("sale_type",saledetails.sale_type);
                    cmd.Parameters.AddWithValue("sale_value",saledetails.sale_value);
                    cmd.Parameters.AddWithValue("sale_description",saledetails.sale_description);
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Sale Record Added Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Sale Not Added Successfully !";
                            return RedirectToAction("Index");
                        }


                    }
                    catch (Exception ex)
                    {

                        TempData["Message"] =ex.Message;
                        return RedirectToAction("Index");

                    }
                }
                catch (AggregateException)
                {
                    TempData["Message"] = "Check Database Connection !";
                    return RedirectToAction("Index");

                }

            }
        }

        public IActionResult Delete(int id)
        {
            string query = @"
                        delete from sales 
                        where sale_id=@id;
                        
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
                        TempData["Message"] = "Sale Deleted !";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Sale Delete Failed !";
                        return RedirectToAction("Index");
                    }

                }
            }
        }


    }
}
