using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestoWebApp.Models;

namespace RestoWebApp.Controllers
{
    public class RestaurantDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RestaurantData/GetRestaurants
        [HttpGet]
        public IEnumerable<RestaurantDto> GetRestaurants()
        {
            // Create a new list to insert restaurants from database into
            List<Restaurant> Restaurants = db.Restaurants.ToList();
            // Start a new list of restaurant data transfer objects
            List<RestaurantDto> RestaurantDtos = new List<RestaurantDto> { };

            foreach(var Restaurant in Restaurants)
            {
                // Loop through database and insert restaurant object attributes into DTO
                RestaurantDto NewRestaurant = new RestaurantDto
                {
                    RestaurantID = Restaurant.RestaurantID,
                    RestaurantName = Restaurant.RestaurantName,
                    RestaurantAddress = Restaurant.RestaurantAddress,
                    RestaurantPhone = Restaurant.RestaurantPhone
                };
                // Add DTO to list then loop back
                RestaurantDtos.Add(NewRestaurant);
            }

            return RestaurantDtos;
        }

        // GET: api/RestaurantData/FindRestaurant/5
        [ResponseType(typeof(RestaurantDto))]
        [HttpGet]
        public IHttpActionResult FindRestaurant(int id)
        {
            Restaurant RestaurantInfo = db.Restaurants.Find(id);
            RestaurantDto SelectedRestaurant = new RestaurantDto
            {
                RestaurantID = RestaurantInfo.RestaurantID,
                RestaurantName = RestaurantInfo.RestaurantName,
                RestaurantAddress = RestaurantInfo.RestaurantAddress,
                RestaurantPhone = RestaurantInfo.RestaurantPhone,
            };

            if (SelectedRestaurant == null)
            {
                return NotFound();
            }

            return Ok(SelectedRestaurant);
        }

        // POST: api/RestaurantData/UpdateRestaurant/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRestaurant(int id, [FromBody]Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant.RestaurantID)
            {
                return BadRequest();
            }

            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RestaurantData/AddRestaurant
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        public IHttpActionResult AddRestaurant([FromBody]Restaurant NewRestaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurants.Add(NewRestaurant);
            db.SaveChanges();

            return Ok(NewRestaurant.RestaurantID);
        }

        // POST: api/RestaurantData/DeleteRestaurant/5
        [HttpPost]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantExists(int id)
        {
            return db.Restaurants.Count(e => e.RestaurantID == id) > 0;
        }
    }
}