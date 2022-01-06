using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Constants
{
    public enum EnumProductType
    {

        [Display(Name = "Car")]
        Car = 1,

        [Display(Name = "Accessories")]
        Accessories = 2,
    }
}