using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace RestoWebApp.Models
{
    public class Owner
    {
        [Key]
        public int OwnerID { get; set; }

        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public string OwnerEmail { get; set; }
        
        public string OwnerAddress { get; set; }

        public string OwnerPhone { get; set; }
    }
    public class OwnerDto
    {
        public int OwnerID { get; set; }
        [DisplayName("First Name")]
        public string OwnerFirstName { get; set; }
        [DisplayName("Last Name")]
        public string OwnerLastName { get; set; }
        [DisplayName("Email")]
        public string OwnerEmail { get; set; }
        [DisplayName("Address")]
        public string OwnerAddress { get; set; }
        [DisplayName("Phone Number")]
        public string OwnerPhone { get; set; }
    }
}