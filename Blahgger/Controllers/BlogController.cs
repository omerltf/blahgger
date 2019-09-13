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
    [Authorize]
    public class BlogController : Controller
    {
        private List<Blog> blogs = new List<Blog>();
        private User myUser;

        public BlogController()
        {
            if (User!=null)
            {
                myUser = new User();
                BlogHelper helper = new BlogHelper();
                string currentUserEmail = User.Identity.Name;
                myUser.Email = currentUserEmail;
                myUser.Id = helper.GetCurrentUserId(currentUserEmail);
                myUser.Password = "";

                if (myUser.Id == -1)
                {
                    //action logout, send to login page
                    FormsAuthentication.SignOut();

                    throw new ApplicationException("Could not find user");
                }
            }
        }
        // GET: Blog
        public ActionResult Index()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Blahgger"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                      SELECT Id, PostCreatorUserId, Text, Title, CreatedOn
                      FROM BlogPosts
                    ";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Blog blog = new Blog();
                    blog.PostCreatorUserId = reader.GetInt32(1);
                    blog.Text = reader.GetString(2);
                    blog.Title = reader.GetString(3);
                    blog.CreatedOn = reader.GetDateTime(4);
                    blogs.Add(blog);
                }

            }
            return View(blogs);
        }

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            //myUser = new User();
            //BlogHelper helper = new BlogHelper();
            //string CurrentUserEmail = User.Identity.Name;
            //myUser.Email = CurrentUserEmail;
            //myUser.Id = helper.GetCurrentUserId(CurrentUserEmail);
            //myUser.Password = "";

            //if (myUser.Id == -1)
            //{
            //    ModelState.AddModelError("", "Unable to find user");
            //    //action logout, send to login page
            //    FormsAuthentication.SignOut();
            //    return RedirectToAction("Login", "Account");
            //}

            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        public ActionResult Create(Blog blog)
        {

            //if (myUser.Id == -1)
            //{
            //    ModelState.AddModelError("", "Unable to find user");
            //    //action logout, send to login page
            //}

            if (ModelState.IsValid)
            {
                DateTime dateTime = DateTime.Now;
                //blog.PostCreatorUserId = currentUserId;
                //blog.CreatedOn = dateTime;
                string connectionString = ConfigurationManager.ConnectionStrings["Blahgger"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                            INSERT INTO BlogPosts(PostCreatorUserId, Text, Title, CreatedOn)
                            VALUES (@PostCreatorUserId, @Text, @Title, @CreatedOn)
                        ";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@PostCreatorUserId", myUser.Id);
                    command.Parameters.AddWithValue("@Text", blog.Text);
                    command.Parameters.AddWithValue("@Title", blog.Title);
                    command.Parameters.AddWithValue("@CreatedOn", dateTime);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Index", "Blog");
            }
            return View(blog);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Blog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Blog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
