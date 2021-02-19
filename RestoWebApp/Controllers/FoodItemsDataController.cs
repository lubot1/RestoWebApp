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
    public class FoodItemsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FoodItemsData/GetFoodItems
        [HttpGet]
        public IEnumerable<FoodItemDto> GetFoodItems()
        {
            List<FoodItem> FoodItems = db.FoodItems.ToList();
            List<FoodItemDto> FoodItemDtos = new List<FoodItemDto> { };
            foreach(var FoodItem in FoodItems)
            {
                FoodItemDto NewFoodItem = new FoodItemDto
                {
                    Restaurant = FoodItem.Restaurant,
                    FoodItemID = FoodItem.FoodItemID,
                    FoodItemName = FoodItem.FoodItemName,
                    FoodItemDesc = FoodItem.FoodItemDesc,
                    FoodItemPrice = FoodItem.FoodItemPrice
                };
                FoodItemDtos.Add(NewFoodItem);
            }

            return FoodItemDtos;
        }

        // GET: api/FoodItemsData/FindFoodItem/5
        [ResponseType(typeof(FoodItemDto))]
        [HttpGet]
        public IHttpActionResult FindFoodItem(int id)
        {
            FoodItem FoodItemInfo = db.FoodItems.Find(id);

            if (FoodItemInfo == null)
            {
                return NotFound();
            }
            FoodItemDto SelectedFoodItem = new FoodItemDto
            {
                Restaurant = FoodItemInfo.Restaurant,
                FoodItemID = FoodItemInfo.FoodItemID,
                FoodItemName = FoodItemInfo.FoodItemName,
                FoodItemDesc = FoodItemInfo.FoodItemDesc,
                FoodItemPrice = FoodItemInfo.FoodItemPrice
            };

            return Ok(SelectedFoodItem);
        }

        // POST: api/FoodItemsData/UpdateFoodItem/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFoodItem(int id, [FromBody]FoodItem foodItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodItem.FoodItemID)
            {
                return BadRequest();
            }

            db.Entry(foodItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
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

        // POST: api/FoodItemsData/AddFoodItem
        [ResponseType(typeof(FoodItem))]
        [HttpPost]
        public IHttpActionResult AddFoodItem([FromBody]FoodItem foodItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FoodItems.Add(foodItem);
            db.SaveChanges();

            return Ok(foodItem.FoodItemID);
        }

        // POST: api/FoodItemsData/DeleteFoodItem/5
        [ResponseType(typeof(FoodItem))]
        [HttpPost]
        public IHttpActionResult DeleteFoodItem(int id)
        {
            FoodItem SelectedFoodItem = db.FoodItems.Find(id);
            if (SelectedFoodItem == null)
            {
                return NotFound();
            }

            db.FoodItems.Remove(SelectedFoodItem);
            db.SaveChanges();

            return Ok(SelectedFoodItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FoodItemExists(int id)
        {
            return db.FoodItems.Count(e => e.FoodItemID == id) > 0;
        }
    }
}