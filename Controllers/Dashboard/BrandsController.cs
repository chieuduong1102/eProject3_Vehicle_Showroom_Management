using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Models;
using eProject3_Vehicle_Showroom_Management.Models.DTO;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class BrandsController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Brands
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BrandName,UrlLogo")] BrandDTO brandDTO)
        {
            if (ModelState.IsValid)
            {
                Brand brand = new Brand();
                brand.BrandName = brandDTO.BrandName;
                if (brandDTO.UrlLogo.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(brandDTO.UrlLogo.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/products-images"), _FileName);
                    brandDTO.UrlLogo.SaveAs(_path);
                    brand.UrlLogo = Extensions.Extension.ConvertToBase64(_path);
                }

                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brandDTO);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BrandName,UrlLogo")] BrandDTO brandDTO)
        {
            if (ModelState.IsValid)
            {
                Brand brand = new Brand();
                brand.BrandName = brandDTO.BrandName;
                if (brandDTO.UrlLogo.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(brandDTO.UrlLogo.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/products-images"), _FileName);
                    brandDTO.UrlLogo.SaveAs(_path);
                    brand.UrlLogo = Extensions.Extension.ConvertToBase64(_path);
                }

                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brandDTO);
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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
