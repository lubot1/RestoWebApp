using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoWebApp.Models
{
    public class Owners
    {
        [Key]
        public int OwnerID { get; set; }

        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public string OwnerEmail { get; set; }
        
        public string OwnerAddress { get; set; }

        public string OwnerPhone { get; set; }
    }
}