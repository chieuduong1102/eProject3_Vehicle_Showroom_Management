using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Constants;
using eProject3_Vehicle_Showroom_Management.Models;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class EmployeesController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).Include(e => e.Showroom);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName");
            ViewBag.ShowroomId = new SelectList(db.Showrooms, "Id", "ShowroomName");
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(EnumLevelEmployee)).OfType<Enum>().Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(EnumLevelEmployee), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Fullname,PhoneNumber,Address,Password,Position,DepartmentId,ShowroomId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewBag.ShowroomId = new SelectList(db.Showrooms, "Id", "ShowroomName", employee.ShowroomId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewBag.ShowroomId = new SelectList(db.Showrooms, "Id", "ShowroomName", employee.ShowroomId);
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(EnumLevelEmployee)).OfType<Enum>().Select(x =>
                                    new SelectListItem
                                    {
                                    Text = Enum.GetName(typeof(EnumLevelEmployee), x),
                                    Value = (Convert.ToInt32(x)).ToString()
                                    }), "Value", "Text");
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Fullname,PhoneNumber,Address,Password,Position,DepartmentId,ShowroomId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentId);
            ViewBag.ShowroomId = new SelectList(db.Showrooms, "Id", "ShowroomName", employee.ShowroomId);

            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
