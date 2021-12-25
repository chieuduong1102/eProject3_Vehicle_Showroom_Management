using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProject3_Vehicle_Showroom_Management.Controllers.UI
{
    public class LoginClientController : Controller
    {
        // GET: LoginClient
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginClient/Create
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

        // GET: LoginClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginClient/Edit/5
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

        // GET: LoginClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginClient/Delete/5
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
