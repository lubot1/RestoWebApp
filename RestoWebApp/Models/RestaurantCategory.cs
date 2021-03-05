using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestoWebApp.Models
{
    public class RestaurantCategory
    {
        [Key]
        public int RestaurantCategoryID { get; set; }

        public string RestaurantCategoryDesc { get; set; }
    }
    public class RestaurantCategoryDto
    {
        public int RestaurantCategoryID { get; set; }
        [DisplayName("Category")]
        public string RestaurantCategoryDesc { get; set; }
    }
}