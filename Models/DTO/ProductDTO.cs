using eProject3_Vehicle_Showroom_Management.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public string YearOfManufacture { get; set; }
        public int? Seats { get; set; }
        public EnumTransmissionType TransmissionType { get; set; }
        public decimal Price { get; set; }
        public EnumProductStatus Status { get; set; }
        public int? Rating { get; set; }

        public HttpPostedFileBase[] Images { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public ProductDTO()
        {
        }
    }
}