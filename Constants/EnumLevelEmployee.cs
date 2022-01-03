using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Constants
{
    public enum EnumLevelEmployee
    {

        [Display(Name = "Employee")]
        Employee = 1,

        [Display(Name = "Admin")]
        Admin = 2,

        [Display(Name = "CEO")]
        CEO = 3,

        [Display(Name = "CoFounder")]
        CoFounder = 4,
    }
}