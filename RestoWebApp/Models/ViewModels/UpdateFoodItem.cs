using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestoWebApp.Models.ViewModels
{
    public class UpdateFoodItem
    {
        public FoodItemDto FoodItem { get; set; }
        // Pull restaurant owners
        public IEnumerable<RestaurantDto> Restaurants { get; set; }
    }
}