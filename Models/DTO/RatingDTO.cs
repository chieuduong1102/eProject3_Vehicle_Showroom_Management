using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Models.DTO
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }
        public string Comments { get; set; }
    }
}