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
using PagedList;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class ProductsController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

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
                    productDTO.CreatedDate = !string.IsNullOrEmpty(x.CreatedDate) ? x.CreatedDate : string.Empty;
                    productDTO.UpdatedDate = !string.IsNullOrEmpty(x.UpdatedDate) ? x.UpdatedDate : string.Empty;
                list.Add(productDTO);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.ProductName.Contains(searchString)).ToList();
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ProductDTO productDTO = new ProductDTO();
            productDTO.Id = product.Id;
            productDTO.ProductName = product.ProductName;
            productDTO.ProductType = db.ProductTypes.Find(product.ProductTypeId).ProductType1;
            productDTO.Brand = db.Brands.Find(product.BrandId).BrandName;
            productDTO.YearOfManufacture = product.YearOfManufacture;
            productDTO.Seats = product.Seats == null ? 0 : (int)product.Seats;
            productDTO.TransmissionType = (EnumTransmissionType)(int)product.TransmissionType;
            productDTO.Price = product.Price;
            productDTO.Status = (EnumProductStatus)product.Status;
            productDTO.Rating = GenerateRaingOfProduct(product.Id);
            productDTO.CreatedDate = product.CreatedDate;
            productDTO.UpdatedDate = string.IsNullOrEmpty(product.UpdatedDate) ? product.UpdatedDate : string.Empty;
            productDTO.UrlImages = db.Images.Where(x => x.ProductId == product.Id).Select(i => i.UrlImage).ToList();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(productDTO);
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
            ViewBag.Brand = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.ProductType = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,ProductType,Brand,YearOfManufacture,Seats,TransmissionType,Price,Status,Images")] ProductDTO productDTO)
        {
            productDTO.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (ModelState.IsValid)
            {
                Product product = new Product();
                if (productDTO.Images.Length > 0 && productDTO.Images.First() != null)
                {
                    foreach (var file in productDTO.Images)
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
                product.Id = productDTO.Id;
                product.ProductName = productDTO.ProductName;
                product.ProductTypeId = Int32.Parse(productDTO.ProductType);
                product.BrandId = Int32.Parse(productDTO.Brand);
                product.YearOfManufacture = productDTO.YearOfManufacture;
                product.Seats = productDTO.Seats;
                product.TransmissionType = (int?)productDTO.TransmissionType;
                product.Price = productDTO.Price;
                product.Status = (int)productDTO.Status;
                product.UpdatedDate = productDTO.UpdatedDate;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productDTO);
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
            if (product != null) { 
                List<Image> images = db.Images.Where(x => x.ProductId == id).ToList();
                images.ForEach(i => db.Images.Remove(i));
                db.Products.Remove(product);
                db.SaveChanges();
            }
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
