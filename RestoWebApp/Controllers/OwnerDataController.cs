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
    public class OwnerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OwnerData/GetOwners
        [HttpGet]
        public IEnumerable<OwnerDto> GetOwners()
        {
            List<Owner> Owners = db.Owners.ToList();
            List<OwnerDto> OwnerDtos = new List<OwnerDto> { };

            foreach(var Owner in Owners)
            {
                OwnerDto NewOwner = new OwnerDto
                {
                    OwnerID = Owner.OwnerID,
                    OwnerFirstName = Owner.OwnerFirstName,
                    OwnerLastName = Owner.OwnerLastName
                };
                OwnerDtos.Add(NewOwner);
            }
            return OwnerDtos;
        }

        // GET: api/OwnerData/GetOwner/5
        [ResponseType(typeof(Owner))]
        [HttpGet]
        public IHttpActionResult GetOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            return Ok(owner);
        }

        // PUT: api/Owners/UpdateOwner/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateOwner(int id, [FromBody]Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owner.OwnerID)
            {
                return BadRequest();
            }

            db.Entry(owner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(id))
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

        // POST: api/Owners/AddOwner
        [ResponseType(typeof(Owner))]
        [HttpPost]
        public IHttpActionResult AddOwner([FromBody]Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owners.Add(owner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = owner.OwnerID }, owner);
        }

        // DELETE: api/Owners/5
        [ResponseType(typeof(Owner))]
        public IHttpActionResult DeleteOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            db.Owners.Remove(owner);
            db.SaveChanges();

            return Ok(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnerExists(int id)
        {
            return db.Owners.Count(e => e.OwnerID == id) > 0;
        }
    }
}