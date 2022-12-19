using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PMS.Models;
using System.Collections.Generic;
using System;
using System.Data;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace PMS.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {

            List<Users> users = new List<Users>();
            //MYSQL Connection
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from users", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //GET DATA
                    Users user = new Users();
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.First_Name = reader["first_name"].ToString();
                    user.Last_Name = reader["last_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Mobile = Convert.ToInt32(reader["mobile"]);
                    user.Address = reader["address"].ToString();

                    users.Add(user);
                }
                reader.Close();
            }
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateSubmit(string fn, string ln, string email, string pwd, int mobile, string add)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO users (`id`, `first_name`, `last_name`, `email`, `password`, `mobile`, `address`) 
                                     VALUES (NULL,@fn,@ln,@email,@pwd,@mobile,@add)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("fn", fn);
                    cmd.Parameters.AddWithValue("ln", ln);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("pwd", pwd);
                    cmd.Parameters.AddWithValue("mobile", mobile);
                    cmd.Parameters.AddWithValue("add", add);
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "User Added Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return new JsonResult("ERROR");
                        }


                    }
                    catch (Exception)
                    {

                        return new JsonResult("ADMIN_DUPLICATED");

                    }
                }
                catch (AggregateException)
                {
                    return new JsonResult("DB_ERROR");

                }

            }
        }

        public IActionResult Edit(int id)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                conn.Open();
                string query = @"SELECT * FROM users WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", id);
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
                    if (user != null)
                    {
                        return View(user);

                    }

                }
                reader.Close();
            }
            TempData["Message"] = "Un Known !";
            return RedirectToAction("Index");
        }
        public IActionResult Save(Users user)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                try
                {
                    conn.Open();
                    string query = @"
                                           UPDATE users
                                           SET first_name = @fn,
                                               last_name = @ln,
                                               email = @email,
                                               password =@pwd,
                                               mobile=@mobile,
                                               address = @add
                                           WHERE id=@id;
                                       "; 
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", user.Id);
                    cmd.Parameters.AddWithValue("fn", user.First_Name);
                    cmd.Parameters.AddWithValue("ln", user.Last_Name);
                    cmd.Parameters.AddWithValue("email", user.Email);
                    cmd.Parameters.AddWithValue("mobile", user.Mobile);
                    cmd.Parameters.AddWithValue("pwd", user.Password);
                    cmd.Parameters.AddWithValue("add", user.Address);
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {

                        var res = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (res == 1)
                        {
                            TempData["Message"] = "User Edited Successfully !";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "User Edit Failed !";
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
                        delete from users 
                        where id=@id;
                        
            ";

            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection("server=localhost;user=root;password='';port=3306;database=pms;"))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id",id);

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


