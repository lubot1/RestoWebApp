using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace RestoWebApp.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        public string RestaurantName { get; set; }

        public string RestaurantAddress { get; set; }

        public string RestaurantPhone { get; set; }

        [ForeignKey("RestaurantCategory")]
        public int RestaurantCategoryID { get; set; }
        public virtual RestaurantCategory RestaurantCategory { get; set; }
    }
    public class RestaurantDto
    {
        public int RestaurantID { get; set; }
        [DisplayName("Restaurant Name")]
        public string RestaurantName { get; set; }
        [DisplayName("Address")]
        public string RestaurantAddress { get; set; }
        [DisplayName("Phone Number")]
        public string RestaurantPhone { get; set; }
        public int OwnerID { get; set; }

        public int RestaurantCategoryID { get; set; }
        [DisplayName("Category")]
        public virtual RestaurantCategory RestaurantCategory { get; set; }
    }
}