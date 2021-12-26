using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            var products = db.Products.ToList();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var x in products)
            {
                ProductDTO productDTO = new ProductDTO();
                    productDTO.Id = x.Id;
                    productDTO.ProductName = x.ProductName;
                    productDTO.ProductType = db.ProductTypes.Find(x.ProductTypeId).ProductType1;
                    productDTO.Brand = db.Brands.Find(x.BrandId).BrandName;
                    productDTO.YearOfManufacture = x.YearOfManufacture;
                    productDTO.Seats = x.Seats == null ? 0 : (int)x.Seats;
                    productDTO.TransmissionType = (EnumTransmissionType)(int)x.TransmissionType;
                    productDTO.Price = x.Price;
                    productDTO.Status = (EnumProductStatus)x.Status;
                    productDTO.Rating = GenerateRaingOfProduct(x.Id);
                    productDTO.CreatedDate = x.CreatedDate;
                    productDTO.UpdatedDate = string.IsNullOrEmpty(x.UpdatedDate) ? x.UpdatedDate : string.Empty;
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
            ViewBag.Brand = new SelectList(db.Brands, "Id", "BrandName");
            ViewBag.ProductType = new SelectList(db.ProductTypes, "Id", "ProductType1");
            ViewBag.TransmissionType = new SelectList(Enum.GetValues(typeof(EnumTransmissionType)).OfType<Enum>().Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(EnumTransmissionType), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
            ViewBag.ProductStatus = new SelectList(Enum.GetValues(typeof(EnumProductStatus)).OfType<Enum>().Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(EnumProductStatus), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,ProductType,Brand,YearOfManufacture,Seats,TransmissionType,Price,Status,Images")] ProductDTO productDTO)
        {
            productDTO.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            productDTO.UpdatedDate = string.Empty;
            if (ModelState.IsValid)
            {
                Product product = new Product();
                if (productDTO.Images.Length > 0)
                {
                    foreach(var file in productDTO.Images)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Content/products-images"), _FileName);
                        file.SaveAs(_path);
                        Image image = new Image();
                        image.ProductId = productDTO.Id;
                        image.UrlImage = Extensions.Extension.ConvertToBase64(_path);
                        db.Images.Add(image);
                    }

                }
                product.ProductName = productDTO.ProductName;
                product.ProductTypeId = Int32.Parse(productDTO.ProductType);
                product.BrandId = Int32.Parse(productDTO.Brand);
                product.YearOfManufacture = productDTO.YearOfManufacture;
                product.Seats = productDTO.Seats;
                product.TransmissionType = (int?)productDTO.TransmissionType;
                product.Price = productDTO.Price;
                product.Status = (int)productDTO.Status;
                product.CreatedDate = productDTO.CreatedDate;
                product.UpdatedDate = productDTO.UpdatedDate;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", productDTO.Brand);
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", productDTO.ProductType);
            return View(productDTO);
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
