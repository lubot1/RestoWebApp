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
                    RestaurantAddress = Restaurant.RestaurantAddress,
                    RestaurantPhone = Restaurant.RestaurantPhone
                };
                // Add DTO to list then loop back
                RestaurantDtos.Add(NewRestaurant);
            }

            return RestaurantDtos;
        }

        // GET: api/RestaurantData/GetRestaurant/5
        [ResponseType(typeof(Restaurant))]
        [HttpGet]
        public IHttpActionResult GetRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
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

        // POST: api/RestaurantData
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        public IHttpActionResult AddRestaurant([FromBody]Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurants.Add(restaurant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurant.RestaurantID }, restaurant);
        }

        // DELETE: api/RestaurantData/5
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
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