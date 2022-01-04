using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Extensions;
using eProject3_Vehicle_Showroom_Management.Models;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class LoginAdminController : Controller
    {
        // GET: LoginAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel emp)
        {
            if (ModelState.IsValid)
            {
                using(eProject3Entities db = new eProject3Entities())
                {
                    string password = Extension.GetMD5(Extension.GetSHA(emp.Password));
                    var employee = db.Employees.Where(x => x.Email.Equals(emp.Email) && x.Password.Equals(password)).FirstOrDefault();
                    if(employee != null)
                    {
                        Session["Admin"] = employee.Fullname;
                        return RedirectToAction("Index","Dashboard");
                    }
                    else
                    {
                        TempData["message"] = "Incorrect username or password";
                        return View(emp);
                    }
                }
            }
            return View(emp);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Index", "LoginAdmin");
        }

        // GET: LoginAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginAdmin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginAdmin/Edit/5
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

        // GET: LoginAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginAdmin/Delete/5
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
