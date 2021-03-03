using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestoWebApp.Models
{
    public class OwnerxRestaurant
    {
        [Key]
        public int OwnerxRestaurantID { get; set; }

        [ForeignKey("Owner")]
        public int OwnerID { get; set; }
        public virtual Owner Owner { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}