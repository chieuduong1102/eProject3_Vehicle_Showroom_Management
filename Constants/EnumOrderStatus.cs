using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Constants
{
    public enum EnumOrderStatus
    {

        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Cancel")]
        Cancel = 1,

        [Display(Name = "Accept")]
        Accept = 2,

        [Display(Name = "Received")]
        Received = 3,
    }
}