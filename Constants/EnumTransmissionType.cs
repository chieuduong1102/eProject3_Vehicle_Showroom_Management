using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Constants
{
    public enum EnumTransmissionType
    {
        [Display(Name = "None")]
        None = 0,

        [Display(Name = "Manual")]
        Manual = 1,

        [Display(Name = "Automatic")]
        Automatic = 2,
    }
}