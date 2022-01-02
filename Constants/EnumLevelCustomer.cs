using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Constants
{
    public enum EnumLevelCustomer
    {

        [Display(Name = "Bronze")]
        Bronze = 1,

        [Display(Name = "Silver")]
        Silver = 2,

        [Display(Name = "Platium")]
        Platium = 3,

        [Display(Name = "Diamond")]
        Diamond = 4,
    }
}