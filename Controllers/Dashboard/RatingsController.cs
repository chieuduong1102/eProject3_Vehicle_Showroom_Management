using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Models;
using eProject3_Vehicle_Showroom_Management.Models.DTO;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class RatingsController : Controller
    {
        private eProject3Entities db = new eProject3Entities();

        // GET: Ratings
        public ActionResult Index()
        {
            List<RatingDTO> ratingDTOs = new List<RatingDTO>();
            var ratings = db.Ratings.ToList();
            List<int> listProductId = ratings.Select(x => x.ProductId).Distinct().ToList();

            foreach (var item in listProductId)
            {
                RatingDTO ratingDTO = new RatingDTO();
                ratingDTO.Id = item;
                ratingDTO.ProductId = item;
                ratingDTO.ProductName = db.Products.Find(item).ProductName;
                ratingDTO.Image = db.Images.Where(i => i.ProductId == item).First().UrlImage.ToString();
                ratingDTO.Rating = (int)(db.Ratings.Where(x => x.ProductId == item).Sum(x => x.Rating1)) / (db.Ratings.Where(x => x.ProductId == item).Count());
                ratingDTOs.Add(ratingDTO);
            }
            return View(ratingDTOs.ToList());
        }

        [HttpPost]
        public JsonResult AddNewRating(RatingDTO r)
        {
            if (Session["Customer"] == null && Request.Cookies["Email"] == null)
            {
                Response.StatusCode = 403;
                return Json("You must login first");
            }
            string email = Session["Customer"]!=null? Session["Customer"].ToString(): Request.Cookies["Email"].Value;
            int customer_id = Convert.ToInt32(db.Customers.Where(a => a.Email.Equals(email)).Select(a => a.Id).Single());
            Rating rating = new Rating()
            {
                ProductId = r.ProductId,
                Rating1 = r.Rating,
                Comments = r.Comments,
                CustomerId = customer_id,
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };
            db.Ratings.Add(rating);
            db.SaveChanges();

            var count = db.Ratings.Where(x => x.ProductId == r.ProductId).Count();
            Response.StatusCode = 200;
            return Json(new{ message = "Add new rating successfully", number = count});
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Rating> ratings = db.Ratings.Where(r => r.ProductId == id).ToList();
            if (ratings == null)
            {
                return HttpNotFound();
            }
            return View(ratings);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Rating1")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", rating.ProductId);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", rating.ProductId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,Rating1")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", rating.ProductId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
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
