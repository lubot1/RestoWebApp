using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoWebApp.Models
{
    public class FoodItems
    {
        [Key]
        public int FoodItemID { get; set; }

        public string FoodItemName { get; set; }

        public string FoodItemDesc { get; set; }

        public float FoodItemPrice { get; set; }
    }
}