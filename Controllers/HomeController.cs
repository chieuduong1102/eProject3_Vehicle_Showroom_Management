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
            if (TempData["statusOrder"] != null)
            {
                ViewBag.statusOrder = TempData["statusOrder"].ToString();
            }

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
            if(Session["Customer"] == null && Request.Cookies["Email"] == null)
            {
                return RedirectToAction("Index", "LoginClient");
            }
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ListProduct(string brand, string transmissionType, string priceCar)
        {
            var listProduct = getProductList();
            if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(transmissionType) && !string.IsNullOrEmpty(priceCar))
            {
                var minPrice = Int32.Parse(priceCar.Split(',')[0]);
                var maxPrice = Int32.Parse(priceCar.Split(',')[1]);
                var listProductFiltered = listProduct.Where(x => x.Brand.Contains(brand) 
                                            && x.TransmissionType.GetDisplayName().Contains(transmissionType) 
                                                    && x.Price >= minPrice && x.Price <= maxPrice).ToList();
                return View(listProductFiltered);
            }
            return View(listProduct);
        }

        public int GenerateRaingOfProduct(int id)
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
                productDTO.Rating = GenerateRaingOfProduct(x.Id);
                productDTO.CreatedDate = x.CreatedDate;
                productDTO.UpdatedDate = string.IsNullOrEmpty(x.UpdatedDate) ? x.UpdatedDate : string.Empty;
                productDTO.Descriptions = x.Descriptions;
                productDTO.UrlImages = db.Images.Where(i => i.ProductId == x.Id).Select(a => a.UrlImage).ToList();
                list.Add(productDTO);
            }

            return list;
        }

        public ActionResult addToCart(int id)
        {
            if (Session["cart"] == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { Product = db.Products.Find(id), Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = db.Products.Find(id), Quantity = 1 });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Cart");
        }

        public ActionResult RemoveCarInCart(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Cart");
        }

        public ActionResult RemoveAllCart()
        {
            Session.Remove("cart");
            return RedirectToAction("Cart");
        }

        private int isExist(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult SubmitCart(string txtAddress, string emailUser, decimal totalPrice, string note)
        {
            try
            {
                string address = txtAddress;
                string email = emailUser;
                decimal total = totalPrice;
                string noteUser = note;
                Order order = new Order();
                order.CustomerId = db.Customers.Where(x => x.Email.Contains(email)).FirstOrDefault().Id;
                order.TotalPrice = total;
                order.DeliveryAddress = address;
                order.Status = (int)EnumOrderStatus.Pending;
                order.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                order.UpdatedDate = string.Empty;

                db.Orders.Add(order);
                db.SaveChanges();


                OrderDetail orderDetail = new OrderDetail();
                if (Session["cart"] != null)
                {
                    foreach (CartItem item in (List<CartItem>)Session["cart"])
                    {
                        orderDetail.OrderId = db.Orders.ToList().LastOrDefault().Id;
                        orderDetail.ProductId = item.Product.Id;
                        orderDetail.Quantily = item.Quantity;
                        orderDetail.TotalPrice = item.Quantity * item.Product.Price;
                        orderDetail.Note = noteUser;
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                }

                TempData["statusOrder"] = 0;

                Session.Remove("cart");
            } catch (Exception ex)
            {
                TempData["statusOrder"] = 1;
            }


            return RedirectToAction("Index");
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