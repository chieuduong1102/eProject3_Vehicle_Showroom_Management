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
using PagedList;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class OrdersController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Orders
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

            var orders = db.Orders.Include(o => o.Customer).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.Id.Equals(DecodeOrderId(searchString)) || s.Customer.PhoneNumber.Contains(searchString)).ToList();
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(orders.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            OrderDTO orderDTO = new OrderDTO();
            orderDTO.Id = order.Id;
            orderDTO.Fullname = order.Customer.Fullname;
            orderDTO.Email = order.Customer.Email;
            orderDTO.PhoneNumber = order.Customer.PhoneNumber;
            orderDTO.DeliveryAddress = order.DeliveryAddress;
            orderDTO.TotalPrice = order.TotalPrice;
            orderDTO.CreatedDate = order.CreatedDate;
            orderDTO.UpdatedDate = order.UpdatedDate;
            orderDTO.Status = order.Status;
            orderDTO.ListOrderDetails = db.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
            if (orderDTO == null)
            {
                return HttpNotFound();
            }
            return View(orderDTO);
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,TotalPrice,DeliveryAddress,CreatedDate,UpdatedDate")] Order order)
        {
            order.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            order.UpdatedDate = string.Empty;
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Email", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Cancel/5
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            order.Status = (int)EnumOrderStatus.Cancel;
            order.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.Entry(order).State = EntityState.Modified;
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

        private static int DecodeOrderId(string id)
        {
            return id.Length > 14 ? Int32.Parse(id.Substring(14)) : 0;
        }
    }
}
