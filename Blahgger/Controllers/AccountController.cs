using Blahgger.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blahgger.Controllers
{
    public class AccountController : Controller
    {
        //GET:
        public ActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                string email = user.Email;
                string password = user.Password;
                string name = user.Name;

                //try
                //{
                string connectionString = ConfigurationManager.ConnectionStrings["Blahgger"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                            INSERT INTO Users(Email, Password, Name)
                            VALUES (@Email, @Password, @Name)
                        ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
                //}
                //catch
                //{
                //    return View(user);
                //}
            }
            return View(user);
        }



        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            User currentUser = new User();

            if (ModelState.IsValidField("Email") && ModelState.IsValidField("Password"))
            {
                //TODO get user record from the database 
                string connectionString = ConfigurationManager.ConnectionStrings["Blahgger"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                      SELECT Id, Email, Password
                      FROM Users
                      where Email=@Email
                    ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        currentUser.Id = reader.GetInt32(0);
                        currentUser.Email = reader.GetString(1);
                        currentUser.Password = reader.GetString(2);
                    }

                }
                //TODO check if the passwords match
                if (!(user.Email == currentUser.Email && user.Password == currentUser.Password))
                {
                    ModelState.AddModelError("", "Login Failed");
                }
            }

            if (ModelState.IsValid)
            {
                //TODO login the user
                FormsAuthentication.SetAuthCookie(user.Email, false);

                //Send them to homepage
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}