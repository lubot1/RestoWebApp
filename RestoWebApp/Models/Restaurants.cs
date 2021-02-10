using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoWebApp.Models
{
    public class Restaurants
    {
        [Key]
        public int RestaurantID { get; set; }

        public string RestaurantAddress { get; set; }

        public string RestaurantPhone { get; set; }
    }
}