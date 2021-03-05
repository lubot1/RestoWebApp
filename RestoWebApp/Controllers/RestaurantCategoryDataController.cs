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
    public class RestaurantCategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        ///  Generates a list of restaurant categories in the database
        /// </summary>
        /// <returns>IEnumerable<RestaurantCategoryDto></returns>
        // GET: api/RestaurantData/GetRestaurantCategories
        [HttpGet]
        public IEnumerable<RestaurantCategoryDto> GetRestaurantCategories()
        {
            // Create a new list to insert restaurants from database into
            List<RestaurantCategory> Categories = db.RestaurantCategories.ToList();
            // Start a new list of restaurant data transfer objects
            List<RestaurantCategoryDto> RestaurantCategoryDtos = new List<RestaurantCategoryDto> { };

            foreach (var Category in Categories)
            {
                // Loop through database and insert restaurant object attributes into DTO
                RestaurantCategoryDto NewRestaurantCategory = new RestaurantCategoryDto
                {
                    RestaurantCategoryID = Category.RestaurantCategoryID,
                    RestaurantCategoryDesc = Category.RestaurantCategoryDesc
                };
                // Add DTO to list then loop back
                RestaurantCategoryDtos.Add(NewRestaurantCategory);
            }

            return RestaurantCategoryDtos;
        }

        // GET: api/RestaurantCategoryData/GetRestaurantCategory/5
        [ResponseType(typeof(RestaurantCategory))]
        public IHttpActionResult GetRestaurantCategory(int id)
        {
            RestaurantCategory restaurantCategory = db.RestaurantCategories.Find(id);
            if (restaurantCategory == null)
            {
                return NotFound();
            }

            return Ok(restaurantCategory);
        }

        // PUT: api/RestaurantCategoryData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantCategory(int id, RestaurantCategory restaurantCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantCategory.RestaurantCategoryID)
            {
                return BadRequest();
            }

            db.Entry(restaurantCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantCategoryExists(id))
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

        // POST: api/RestaurantCategoryData
        [ResponseType(typeof(RestaurantCategory))]
        public IHttpActionResult PostRestaurantCategory(RestaurantCategory restaurantCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantCategories.Add(restaurantCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantCategory.RestaurantCategoryID }, restaurantCategory);
        }

        // DELETE: api/RestaurantCategoryData/5
        [ResponseType(typeof(RestaurantCategory))]
        public IHttpActionResult DeleteRestaurantCategory(int id)
        {
            RestaurantCategory restaurantCategory = db.RestaurantCategories.Find(id);
            if (restaurantCategory == null)
            {
                return NotFound();
            }

            db.RestaurantCategories.Remove(restaurantCategory);
            db.SaveChanges();

            return Ok(restaurantCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantCategoryExists(int id)
        {
            return db.RestaurantCategories.Count(e => e.RestaurantCategoryID == id) > 0;
        }
    }
}