using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProject3_Vehicle_Showroom_Management.Controllers.UI
{
    public class RegisterClientController : Controller
    {
        // GET: RegisterClient
        public ActionResult Index()
        {
            return View();
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
