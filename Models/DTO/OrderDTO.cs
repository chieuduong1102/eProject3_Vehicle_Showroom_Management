using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryAddress { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public int Status { get; set; }
        public List<OrderDetail> ListOrderDetails { get; set; }
    }
}