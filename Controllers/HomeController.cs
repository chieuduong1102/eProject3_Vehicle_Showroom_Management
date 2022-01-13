using eProject3_Vehicle_Showroom_Management.Constants;
using eProject3_Vehicle_Showroom_Management.Models;
using eProject3_Vehicle_Showroom_Management.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using PagedList;

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
            productDTO.Rating = GenerateRatingOfProduct(product.Id);
            productDTO.Descriptions = product.Descriptions;
            productDTO.CreatedDate = product.CreatedDate;
            productDTO.UpdatedDate = string.IsNullOrEmpty(product.UpdatedDate) ? product.UpdatedDate : string.Empty;
            productDTO.UrlImages = db.Images.Where(x => x.ProductId == product.Id).Select(i => i.UrlImage).ToList();

            ViewBag.ListRating = db.Ratings.Where(x => x.ProductId == id).ToList();
            var listProductRelated = db.Products.Where(x => x.Brand.BrandName.Contains(productDTO.Brand) && x.ProductType.ProductType1.Contains("Car")).ToList();

            foreach (var item in listProductRelated)
            {
                item.Images = db.Images.Where(i => i.ProductId == item.Id).ToList();
            }
            ViewBag.ListProductRelated = listProductRelated.ToList();
            if (product == null)
            {
                return RedirectToAction("Error");
            }
            ViewBag.ProductId = product.Id;
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

        public ActionResult ListProduct(string o, string brand, string transmissionType, string priceCar, string priceAccessories,int? page)
        {
            int pageSize = 5;
            var listProducts = db.Products.ToList();
            ViewBag.listBrands = db.Brands.ToList();
            ViewBag.minPriceCar = listProducts.Where(x => x.ProductTypeId.Equals((int)EnumProductType.Car)).OrderBy(x => x.Price).FirstOrDefault().Price;
            ViewBag.maxPriceCar = listProducts.Where(x => x.ProductTypeId.Equals((int)EnumProductType.Car)).OrderBy(x => x.Price).LastOrDefault().Price;
            ViewBag.minPriceAccessories = listProducts.Where(x => x.ProductTypeId.Equals((int)EnumProductType.Accessories)).OrderBy(x => x.Price).FirstOrDefault().Price;
            ViewBag.maxPriceAccessories = listProducts.Where(x => x.ProductTypeId.Equals((int)EnumProductType.Accessories)).OrderBy(x => x.Price).LastOrDefault().Price;
            var listProduct = getProductList();
            //var urlHasOrderBy = Request.Url.ToString();
            //var orderBy = urlHasOrderBy.Split('o').Last().Split('=').Last();
            string orderBy = o;
            if (Request.Url.ToString().Contains(EnumProductType.Accessories.GetDisplayName()))
            {
                ViewBag.NoResult = string.Empty;
                if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(priceAccessories))
                {
                    var minPrice = Int32.Parse(priceAccessories.Split(',')[0]);
                    var maxPrice = Int32.Parse(priceAccessories.Split(',')[1]);
                    var listProductFiltered = listProduct.Where(x => x.ProductType.Contains(EnumProductType.Accessories.GetDisplayName())
                                                           && x.Brand.Contains(brand)
                                                        && x.Price >= minPrice && x.Price <= maxPrice).ToList();
                    if (listProductFiltered.Count() == 0)
                    {
                        ViewBag.NoResult = "No matching results were found";
                        return View(listProductFiltered.ToPagedList(page ?? 1, pageSize));
                    }
                    ViewBag.NoResult = string.Empty;
                    switch (orderBy)
                    {
                        case "Name":
                            return View(listProductFiltered.OrderBy(x => x.ProductName).ToPagedList(page ?? 1, pageSize));
                        case "Price":
                            return View(listProductFiltered.OrderBy(x => x.Price).ToPagedList(page ?? 1, pageSize));
                        case "New":
                            return View(listProductFiltered.OrderBy(x => x.YearOfManufacture).ToPagedList(page ?? 1, pageSize));
                    }
                    return View(listProductFiltered.ToPagedList(page ?? 1, pageSize));
                }
                switch (orderBy)
                {
                    case "Name":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Accessories.GetDisplayName())).OrderBy(x=>x.ProductName).ToPagedList(page ?? 1, pageSize));
                    case "Price":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Accessories.GetDisplayName())).OrderBy(x => x.Price).ToPagedList(page ?? 1, pageSize));
                    case "New":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Accessories.GetDisplayName())).OrderBy(x => x.YearOfManufacture).ToPagedList(page ?? 1, pageSize));
                    default:
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Accessories.GetDisplayName())).ToPagedList(page ?? 1, pageSize));
                }
            }
            if (Request.Url.ToString().Contains(EnumProductType.Car.GetDisplayName()))
            {
                if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(transmissionType) && !string.IsNullOrEmpty(priceCar))
                {
                    var minPrice = Int32.Parse(priceCar.Split(',')[0]);
                    var maxPrice = Int32.Parse(priceCar.Split(',')[1]);
                    var listProductFiltered = listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())
                                                && x.Brand.Contains(brand)
                                                && x.TransmissionType.GetDisplayName().Contains(transmissionType)
                                                        && x.Price >= minPrice && x.Price <= maxPrice).ToList();
                    if (listProductFiltered.Count() == 0)
                    {
                        ViewBag.NoResult = "No matching results were found";
                        return View(listProductFiltered.ToPagedList(page??1, pageSize));
                    }
                    ViewBag.NoResult = string.Empty;
                    switch (orderBy)
                    {
                        case "Name":
                            return View(listProductFiltered.OrderBy(x => x.ProductName).ToPagedList(page ?? 1, pageSize));
                        case "Price":
                            return View(listProductFiltered.OrderBy(x => x.Price).ToPagedList(page ?? 1, pageSize));
                        case "New":
                            return View(listProductFiltered.OrderBy(x => x.YearOfManufacture).ToPagedList(page ?? 1, pageSize));
                    }
                    return View(listProductFiltered.ToPagedList(page ?? 1, pageSize));
                }
                switch (orderBy)
                {
                    case "Name":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())).OrderBy(x => x.ProductName).ToPagedList(page ?? 1, pageSize));
                    case "Price":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())).OrderBy(x => x.Price).ToPagedList(page ?? 1, pageSize));
                    case "New":
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())).OrderBy(x => x.YearOfManufacture).ToPagedList(page ?? 1, pageSize));
                    default:
                        return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())).ToPagedList(page ?? 1, pageSize));
                }
            }
            return View(listProduct.Where(x => x.ProductType.Contains(EnumProductType.Car.GetDisplayName())).ToPagedList(page ?? 1, pageSize));
        }

        public int GenerateRatingOfProduct(int id)
        {
            var listRating = db.Ratings.Where(x => x.ProductId == id).Select(x => x.Rating1).ToList();
            if (listRating.Count > 0)
            {
                return (int)listRating.Sum() / listRating.Count();
            }
            return 0;
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
                productDTO.Rating = GenerateRatingOfProduct(x.Id);
                productDTO.CreatedDate = x.CreatedDate;
                productDTO.UpdatedDate = string.IsNullOrEmpty(x.UpdatedDate) ? x.UpdatedDate : string.Empty;
                productDTO.Descriptions = x.Descriptions;
                productDTO.UrlImages = db.Images.Where(i => i.ProductId == x.Id).Select(a => a.UrlImage).ToList();
                list.Add(productDTO);
            }

            return list;
        }
    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
              .GetMember(enumValue.ToString())
              .First()
              .GetCustomAttribute<DisplayAttribute>()
              ?.GetName();
        }
    }
}
