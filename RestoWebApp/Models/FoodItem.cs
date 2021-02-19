using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace RestoWebApp.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemID { get; set; }

        public string FoodItemName { get; set; }

        public string FoodItemDesc { get; set; }

        public float FoodItemPrice { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
    public class FoodItemDto
    {
        public int FoodItemID { get; set; }
        [DisplayName("Name")]
        public string FoodItemName { get; set; }
        [DisplayName("Description")]
        public string FoodItemDesc { get; set; }
        [DisplayName("Price")]
        public float FoodItemPrice { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}