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
using eProject3_Vehicle_Showroom_Management.Models.DTO;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class ProductsController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.ProductType);
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var x in products)
            {
                ProductDTO productDTO = new ProductDTO(
                    x.Id,
                    x.ProductName,
                    db.ProductTypes.Where(p => p.Id == x.Id).Select(p => p.ProductType1).ToString(),
                    db.Brands.Where(b => b.Id == x.Id).Select(b => b.BrandName).ToString(),
                    x.YearOfManufacture,
                    x.Seats,
                    x.TransmissionType == (int)EnumTransmissionType.Manual
                                        ? Enum.GetName(typeof(EnumTransmissionType), EnumTransmissionType.Manual)
                                        : Enum.GetName(typeof(EnumTransmissionType), EnumTransmissionType.Automatic),
                    x.Price,
                    GenerateProductStatusFromEnum(x.Status),
                    GenerateRaingOfProduct(x.Id),
                    GenerateImagesOfProduct(x.Id),
                    x.CreatedDate,
                    x.UpdatedDate
                );
                list.Add(productDTO);
            }
            return View(list);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName");
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,ProductTypeId,BrandId,YearOfManufacture,Seats,TransmissionType,Price,Status,CreatedDate,UpdatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,ProductTypeId,BrandId,YearOfManufacture,Seats,TransmissionType,Price,Status,CreatedDate,UpdatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        private string GenerateProductStatusFromEnum(int productStatus)
        {
            switch (productStatus)
            {
                case (int)EnumProductStatus.Unavailable:
                    return Enum.GetName(typeof(EnumProductStatus), EnumProductStatus.Unavailable);
                case (int)EnumProductStatus.Available:
                    return Enum.GetName(typeof(EnumProductStatus), EnumProductStatus.Available);
                case (int)EnumProductStatus.Commingsoon:
                    return Enum.GetName(typeof(EnumProductStatus), EnumProductStatus.Commingsoon);
                default:
                    return string.Empty;
            }
        }

        private int GenerateRaingOfProduct(int id)
        {
            return (int)db.Ratings.Where(x => x.ProductId == id).Select(x => x.Rating1).ToList().Sum();
        }

        private List<string> GenerateImagesOfProduct(int id)
        {
            return db.Images.Where(x => x.ProductId == id).Select(x => x.UrlImage).ToList();
        }
    }
}
