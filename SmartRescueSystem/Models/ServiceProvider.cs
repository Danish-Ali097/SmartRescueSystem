//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartRescueSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ServiceProvider
    {
        public int Id { get; set; }
        [Required, Display(Name="Organization Name")]
        public string Name { get; set; }
        [Required, Display(Name ="Service Catagory")]
        public Nullable<int> Catagory_Id { get; set; }
        [Required, Display(Name="Mobile Number"), RegularExpression(@"^([0]{1})+([3]{1})+([0-9]{9})$", ErrorMessage = "phone number should be in 03xxxxxxxxx format")]
        public string PhoneNumber { get; set; }
        [Required, Display(Name ="Fax Number") ]
        public string Fax { get; set; }
        [Required]
        public string Address { get; set; }
        public string Sgn_Status { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
