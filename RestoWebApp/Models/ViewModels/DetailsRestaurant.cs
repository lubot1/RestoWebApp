using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestoWebApp.Models.ViewModels
{
    public class DetailsRestaurant
    {
        public OwnerDto Owner { get; set; }
        public IEnumerable<RestaurantDto> Restaurants { get; set; }
    }
}