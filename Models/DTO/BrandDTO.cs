using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Models.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Missing Name of Brand")]
        public string BrandName { get; set; }

        public HttpPostedFileBase UrlLogo { get; set; }
    }
}