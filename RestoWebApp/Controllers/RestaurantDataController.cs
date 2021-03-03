using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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

        /// <summary>
        ///  Generates a list of restaurants in the database
        /// </summary>
        /// <returns>IEnumerable<RestaurantDto></returns>
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

        /// <summary>
        /// Retrieves restaurant info from the database using a data transfer object
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Data transfer object with restaurant info</returns>
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
        // Having a little trouble getting info using a bridging table, I need to do more research on this
        //[HttpGet]
        //public IHttpActionResult FindRestaurantOwners(int id)
        //{
        //   List<Owner> RestaurantOwners = db.Owners
        //        .Where(o => o.Restaurants.Any(r => r.RestaurantID))

        //}

        /// <summary>
        /// Handles changes to restaurant information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="restaurant"></param>
        /// <returns>Http Status Code</returns>
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

        /// <summary>
        /// Retrieves information about a new restaurant and inserts a new Restaurant into the database
        /// </summary>
        /// <param name="NewRestaurant"></param>
        /// <returns>Restaurant ID</returns>
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

        [HttpPost]
        public IHttpActionResult AssociateRestaurantOwner([FromBody] RestaurantDto NewRestaurantOwner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OwnerxRestaurant ownerxRestaurant = new OwnerxRestaurant
            {
                RestaurantID = NewRestaurantOwner.RestaurantID,
                OwnerID = NewRestaurantOwner.OwnerID
            };
            db.OwnerxRestaurants.Add(ownerxRestaurant);
            db.SaveChanges();

            return Ok(ownerxRestaurant.RestaurantID);
        }

        /// <summary>
        /// Deletes a restaurant from the database using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP OK result</returns>
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