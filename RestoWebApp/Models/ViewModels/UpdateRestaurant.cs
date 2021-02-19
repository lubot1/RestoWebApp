using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestoWebApp.Models.ViewModels
{
    public class UpdateRestaurant
    {
        //Pull restaurant info
        public RestaurantDto Restaurant { get; set; }
        // Pull restaurant owners
        public IEnumerable<OwnerDto> RestaurantOwners { get; set; }
        // Pull full list of owners for checklist
        public IEnumerable<OwnerDto> OwnersList { get; set; }
    }
}