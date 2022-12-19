using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;

namespace PMS.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {

            List<Company> company = new();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from company", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Company getcompany = new();
                    getcompany.Id = Convert.ToInt32(reader["id"]);
                    getcompany.Name = reader["company_name"].ToString();
                    getcompany.Contact = Convert.ToInt32(reader["company_tel"]);
                    getcompany.Address = reader["company_address"].ToString();
                    company.Add(getcompany);
                }
                reader.Close();
            }
            return View(company);
        }

        public IActionResult Delete(int id)
        {
            string query = @"
                        delete from company 
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
                        TempData["Message"] = "Company Deleted !";
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

        public IActionResult CreateSubmit(Company company)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO company (id,company_name,company_tel, company_address) 
                                     VALUES (NULL,@name,@tel,@add);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", company.Name);
                    cmd.Parameters.AddWithValue("@tel", company.Contact);
                    cmd.Parameters.AddWithValue("@add", company.Address);

                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "Company Added Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "Error!";
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

        public IActionResult Edit(int Id) 
        {
            TempData["ID"] = Id;
            return View(); 
        }

        public IActionResult Save(Company com)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"
                                          UPDATE company SET company_tel = @tel, company_address = @add WHERE company.id = @id;
                                       ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", com.Id);
                    cmd.Parameters.AddWithValue("tel", com.Contact);
                    cmd.Parameters.AddWithValue("add", com.Address);

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
