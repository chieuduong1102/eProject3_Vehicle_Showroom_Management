//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eProject3_Vehicle_Showroom_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public Nullable<int> Position { get; set; }
        public int DepartmentId { get; set; }
        public int ShowroomId { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Showroom Showroom { get; set; }
    }
}
