using eProject3_Vehicle_Showroom_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Extensions;

namespace eProject3_Vehicle_Showroom_Management.Controllers.UI
{
    public class RegisterClientController : Controller
    {
        // GET: RegisterClient
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterViewModel cus)
        {
            if (ModelState.IsValid)
            {
                using (eProject3Entities db = new eProject3Entities())
                {
                    var check = db.Customers.FirstOrDefault(s => s.Email == cus.Email);
                    if (check == null)
                    {
                        Customer customer = new Customer
                        {
                            Email = cus.Email,
                            Fullname = cus.Fullname,
                            PhoneNumber = cus.PhoneNumber,
                            Address = cus.Address,
                            Password = Extension.GetMD5(Extension.GetSHA(cus.Password)),
                            Levels = 1
                        };
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index","LoginClient");
                    }
                    else
                    {
                        TempData["message"] = "Email is already exists";
                        return View();
                    }
                }
            }
            return View(cus);
        }

        // GET: RegisterClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterClient/Create
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

        // GET: RegisterClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterClient/Edit/5
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

        // GET: RegisterClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterClient/Delete/5
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
