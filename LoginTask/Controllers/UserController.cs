using LoginTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace LoginTask.Controllers
{
    public class UserController : Controller
    {
        const string errorMessage = "";
        /*DbUserSignUpLoginEntities db = new DbUserSignUpLoginEntities();*/
        DbUserSignUpLoginEntities1 db = new DbUserSignUpLoginEntities1();
        // GET: User
        public ActionResult Index()
        {
            List<TblUser> obj = new List<TblUser> ();
            obj = db.TblUsers.ToList();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(string userName)
        {

            /*DbUserSignUpLoginEntities obj = new DbUserSignUpLoginEntities();
            TblUser user = new TblUser();
            user = obj.Database.SqlQuery("exec CallMade", userName).ToList();*/
            /*var user = db.SearchUser(userName).ToList();
            List<TblUser> dta = new List<TblUser> ();
            dta = user;*/
            List<TblUser> users = new List<TblUser>();
            List<SearchUser_Result> data = new List<SearchUser_Result> ();
            data = db.SearchUser(userName).ToList();
            foreach(SearchUser_Result dt in data)
            {
                TblUser user = new TblUser();
                user.UserName = dt.UserName;
                user.UserPhone = dt.UserPhone;
                user.UserEmail = dt.UserEmail;
                user.UserAddress = dt.UserAddress;
                users.Add(user);
            }

            /*IEnumerable<TblUser> userData = db.Database.SqlQuery<TblUser>("exec SearchUser", userName).ToList();*/
            return View(users);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TblUser request)
        {
            if (db.TblUsers.Any(x => x.UserEmail == request.UserEmail))
            {
                ViewBag.Notification = "*This account has already existed!";
                return View();
            }
            else
            {
                /*request.UserPassword = encoder.Encode(request.UserPassword);*/
                var pss = Helper.Encrypt(request.UserPassword);
                TblUser obj = new TblUser();
                /*request.UserPassword = pss;*/
                obj.UserName = request.UserName;
                obj.UserEmail = request.UserEmail;
                obj.UserPhone = request.UserPhone;
                obj.UserAddress = request.UserAddress;
                obj.UserPassword = pss;
                obj.RePassword = pss;

                db.TblUsers.Add(obj);
                db.SaveChanges();

                /*Session["UserId"] = request.UserID.ToString();
                Session["UserName"] = request.UserName.ToString();*/
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TblUser requrest)
        {
            string pss = Helper.Encrypt(requrest.UserPassword);
            var checkLogin = db.TblUsers.Where(x => x.UserEmail.Equals(requrest.UserEmail) && x.UserPassword.Equals(pss)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["UserId"] = requrest.UserID.ToString();
                Session["UserName"] = checkLogin.UserName.ToString();
                return RedirectToAction("Index", User);
            }
            else
            {
                ViewBag.Notification = "Invalid crecentials!";
            }
            return View();
        }

    }
}