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
        public Nullable<int> Seats { get; set; }
        public string TransmissionType { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please select files.")]
        [Display(Name = "Browse File")]
        public List<string> Images { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public ProductDTO(int id, string productName, string productType, string brand, string yearOfManufacture, int? seats, string transmissionType, decimal price, string status, int rating, List<string> images, string createdDate, string updatedDate)
        {
            Id = id;
            ProductName = productName;
            ProductType = productType;
            Brand = brand;
            YearOfManufacture = yearOfManufacture;
            Seats = seats;
            TransmissionType = transmissionType;
            Price = price;
            Status = status;
            Rating = rating;
            Images = images;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public ProductDTO()
        {
        }
    }
}