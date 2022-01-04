﻿using eProject3_Vehicle_Showroom_Management.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProject3_Vehicle_Showroom_Management.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        // GET: Dashboard

        [IsAdminAttribute]
        public ActionResult Index()
        {
            return View();
        }
    }
}