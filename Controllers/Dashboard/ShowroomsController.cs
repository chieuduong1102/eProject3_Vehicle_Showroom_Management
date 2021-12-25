using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Models;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class ShowroomsController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Showrooms
        public ActionResult Index()
        {
            return View(db.Showrooms.ToList());
        }

        // GET: Showrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // GET: Showrooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Showrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShowroomName,Address,PhoneNumber,Email,Hotline")] Showroom showroom)
        {
            if (ModelState.IsValid)
            {
                db.Showrooms.Add(showroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(showroom);
        }

        // GET: Showrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // POST: Showrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShowroomName,Address,PhoneNumber,Email,Hotline")] Showroom showroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(showroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(showroom);
        }

        // GET: Showrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // POST: Showrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Showroom showroom = db.Showrooms.Find(id);
            db.Showrooms.Remove(showroom);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
