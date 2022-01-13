using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProject3_Vehicle_Showroom_Management.Models;
using eProject3_Vehicle_Showroom_Management.Extensions;
using System.Web.Security;
using System.Text;

namespace eProject3_Vehicle_Showroom_Management.Controllers.UI
{
    public class LoginClientController : Controller
    {
        // GET: LoginClient
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel cus)
        {
            if (ModelState.IsValid)
            {
                using (eProject3Entities db = new eProject3Entities())
                {
                    string password = Extension.GetMD5(Extension.GetSHA(cus.Password));
                    var customer = db.Customers.Where(x => x.Email.Equals(cus.Email) && x.Password.Equals(password)).FirstOrDefault();
                    if (customer != null)
                    {
                        if (!cus.RememberMe)
                        {
                            Session["Customer"] = customer.Email;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Session["Customer"] = customer.Email;
                            string email = customer.Email;
                            //byte[] data = MachineKey.Protect(Encoding.UTF8.GetBytes("ok"), "a token");
                            //String CookieVal = Extension.FromBytesToBase64(data);
                            HttpCookie cookie = new HttpCookie("Email")
                            {
                                Value = email,
                                Expires = DateTime.Now.AddDays(1)
                            };
                            HttpContext.Response.Cookies.Add(cookie);
                            //string username = Encoding.UTF8.GetString(MachineKey.Unprotect(Request.Cookies(cookieName).Value.FromBase64ToBytes, "a token"));
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Incorrect username or password";
                        return View(cus);
                    }
                }
            }
            return View(cus);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Customer"] = null;
            if (Request.Cookies["Email"] != null)
            {
                Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "LoginClient");
        }

        // GET: LoginClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginClient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginClient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginClient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
