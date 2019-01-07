using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using retry.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace retry.Controllers
{
    public class HomeController : Controller
    {
        string connString = "server=127.0.0.1;port=3306;user id=root;password=t5906503;database=mvc;charset=utf8;";
        MySqlConnection conn = new MySqlConnection();

        [HttpPost]
        public ActionResult Transcripts(Student model)
        {
            string id = model.id;
            string name = model.name;
            int score = model.score;
            Student data = new Student(id, name, score);
            return View(data);
        }

        public ActionResult Transcripts(string id, string name, int score)
        {
            Student data = new Student(id, name, score);
            return View(data);
        }

        [HttpPost]
        public ActionResult Account(Accunt model)
        {

            string account = model.account;
            string password = model.password;

            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"SELECT `id` FROM userdata WHERE `username` = " + account + ";";

            MySqlCommand comp = new MySqlCommand(sql, conn);
            MySqlDataReader n = comp.ExecuteReader();
            if (n.HasRows)
            {
                ViewBag.account = 1;
                conn.Close();

                Response.Redirect("Index");
                return new EmptyResult();
            }
            else
            {
                ViewBag.account = 0;
                conn.Close();
                ViewBag.suc = 1;
                Response.Redirect("Index?suc=0");
                return new EmptyResult();
            }
            /*
            string sql = @"SELECT `id` FROM userdata WHERE `username` = " + account + ";";

            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(dt);

            ViewBag.DT = dt;
            */


            sql = @"INSERT INTO `userdata` (`username`, `password`) VALUES
                           ('"+ account+"', '"+ password+"')";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int index = cmd.ExecuteNonQuery();
            bool success = false;
            if (index > 0)
                success = true;
            else
                success = false;
            ViewBag.Success = success;
            //--------------------------------------------//
            

            conn.Close();
            return View();
        }

        public ActionResult Index()
        {
            /*
            conn.ConnectionString = connString;
            string sql = @" SELECT `id`, `friend` FROM `friend`";

            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(dt);

            ViewBag.DT = dt;
            */
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(Accunt A)
        {
            string account = A.account;
            ViewBag.account = account;
            return View();

            
        }

        public ActionResult About()
        {
            conn.ConnectionString = connString;
            string sql = @" SELECT `id`, `friend` FROM `friend`";

            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(dt);

            ViewBag.DT = dt;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}