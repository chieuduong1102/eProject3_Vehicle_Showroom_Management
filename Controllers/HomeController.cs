using eProject3_Vehicle_Showroom_Management.Constants;
using eProject3_Vehicle_Showroom_Management.Models;
using eProject3_Vehicle_Showroom_Management.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProject3_Vehicle_Showroom_Management.Controllers
{
    public class HomeController : Controller
    {

        private eProject3Entities db = new eProject3Entities();

        public ActionResult Index()
        {
            var listProduct = getProductList();
            ViewBag.listBrands = db.Brands.ToList();
            ViewBag.ListCEO = db.Employees.Where(e => e.Position == (int)EnumLevelEmployee.CEO || e.Position == (int)EnumLevelEmployee.CoFounder).ToList();
            ViewBag.Showrooms = db.Showrooms.ToList();
            return View(listProduct);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error");
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
            productDTO.Descriptions = product.Descriptions;
            productDTO.CreatedDate = product.CreatedDate;
            productDTO.UpdatedDate = string.IsNullOrEmpty(product.UpdatedDate) ? product.UpdatedDate : string.Empty;
            productDTO.UrlImages = db.Images.Where(x => x.ProductId == product.Id).Select(i => i.UrlImage).ToList();

            ViewBag.ListRating = db.Ratings.Where(x => x.ProductId == id).ToList();
            var listProductRelated = db.Products.Where(x => x.Brand.BrandName.Contains(productDTO.Brand)).ToList();
            foreach(var item in listProductRelated)
            {
                item.Images = db.Images.Where(i => i.ProductId == item.Id).ToList();
            }
            ViewBag.ListProductRelated = listProductRelated.ToList();
            if (product == null)
            {
                return RedirectToAction("Error");
            }

            return View(productDTO);
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ListProduct()
        {
            var listProduct = getProductList();
            return View(listProduct);
        }

        public int GenerateRaingOfProduct(int id)
        {
            var listRating = db.Ratings.Where(x => x.ProductId == id).Select(x => x.Rating1).ToList();
            return (int)listRating.Sum()/listRating.Count();
        }

        public List<ProductDTO> getProductList()
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
                productDTO.Descriptions = x.Descriptions;
                productDTO.UrlImages = db.Images.Where(i => i.ProductId == x.Id).Select(a => a.UrlImage).ToList();
                list.Add(productDTO);
            }

            return list;
        }
    }
}